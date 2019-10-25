using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using ComposerCore.Utility;

namespace ComposerCore.Implementation
{
    public class GenericComponentRegistration : ComponentBuilderRegistration
    {
        /// <summary>
        /// Generic type definition 
        /// -> 
        /// Constructed (bound) generic type (obtained from base classes and interfaces)
        /// </summary>
        private readonly IDictionary<Type, Type> _openToBoundContractTypeMap;

        private readonly ConcurrentDictionary<Type, Type> _closedContractToComponentMap;
        private readonly ConcurrentDictionary<Type, ConcreteTypeRegistration> _subRegistrations;
        
        public GenericComponentRegistration(Type targetType) : base(targetType)
        {
            if (!targetType.IsOpenGenericType())
                throw new ArgumentException("TargetType in GenericLocalComponentFactory should be an open generic type.");

            _openToBoundContractTypeMap = new Dictionary<Type, Type>();
            _closedContractToComponentMap = new ConcurrentDictionary<Type, Type>();
            _subRegistrations = new ConcurrentDictionary<Type, ConcreteTypeRegistration>();
        }

        public void AddOpenGenericContractType(Type openContractType)
        {
            EnsureNotInitialized();
            
            if (!openContractType.IsOpenGenericType())
            {
                throw new ArgumentException($"The contract type {openContractType.FullName} is not an open generic type.");
            }

            var boundGenericContract = TargetType
                .GetBaseTypes(true)
                .Concat(TargetType.GetInterfaces())
                .FirstOrDefault(t => t.IsGenericType && t.GetGenericTypeDefinition() == openContractType);

            if (boundGenericContract == null)
                throw new CompositionException($"The open contract type {openContractType.FullName} could not be " +
                                               $"found in the hierarchy of target type {TargetType.FullName}");

            var unresolvableGenericArgs = 
                TargetType.GetGenericArguments().Except(boundGenericContract.GetGenericArguments()).ToArray();
            if (unresolvableGenericArgs.Any())
            {
                var unresolvableGenericArgsString =
                    string.Join(";\n", unresolvableGenericArgs.Select(a => a.ToString()));
                throw new CompositionException($"The contract type {openContractType.FullName} does not contain " +
                                               "enough type arguments to allow constructing a completely closed " +
                                               $"component type out of {TargetType.FullName}. The missing type " +
                                               $"arguments are:\n{unresolvableGenericArgsString}");
            }

            if (!_openToBoundContractTypeMap.ContainsKey(openContractType))
                _openToBoundContractTypeMap.Add(openContractType, boundGenericContract);
            
            var contractIdentity = new ContractIdentity(openContractType, DefaultContractName);
            if (!_contracts.Contains(contractIdentity))
                _contracts.Add(contractIdentity);
        }

        public override void AddContract(ContractIdentity contract)
        {
            AddOpenGenericContractType(contract.Type);
            _contracts.Add(contract);
        }

        public override bool IsResolvable(Type contractType)
        {
            if (!contractType.IsGenericType || contractType.ContainsGenericParameters)
                return false;

            return MapToClosedComponentType(contractType) != null;
        }

        public override object GetComponent(ContractIdentity contract, IComposer dependencyResolver)
        {
            if (!contract.Type.IsGenericType)
                throw new CompositionException("Requested contract " + contract.Type.Name + " is not a generic type.");

            if (contract.Type.ContainsGenericParameters)
                throw new CompositionException("Requested contract " + contract.Type.Name +
                                               " is not fully constructed and closed.");

            var closedTargetType = MapToClosedComponentType(contract.Type);
            if (closedTargetType == null)
                return null;
            
            var subRegistration = _subRegistrations.GetOrAdd(closedTargetType, type =>
            {
                var newSubRegistration = new ConcreteTypeRegistration(type);
                newSubRegistration.CopyConfigFrom(this);
                newSubRegistration.AddContractType(contract.Type);
                newSubRegistration.SetAsRegistered(RegistrationContext);
                return newSubRegistration;
            });

            return subRegistration.GetComponent(contract, dependencyResolver);
        }

        public override object CreateComponent(ContractIdentity contract, IComposer dependencyResolver)
        {
            throw new InvalidOperationException(
                "CreateComponent should never be called on the GenericComponentRegistration class. Instead, " +
                "calling its GetComponent will create sub-registrations for closed generic types and call them " +
                "instead.");
        }

        protected override void ReadContractsFromTarget()
        {
            var boundGenericContracts = ComponentContextUtils.FindContracts(TargetType)
                .Where(t => t.IsOpenGenericType());

            foreach (var boundGenericContract in boundGenericContracts)
            {
                var openContract = boundGenericContract.GetGenericTypeDefinition();
                _openToBoundContractTypeMap.Add(openContract, boundGenericContract);
                _contracts.Add(new ContractIdentity(openContract, DefaultContractName));
            }
        }

        private Type MapToClosedComponentType(Type contractType)
        {
            if (!contractType.IsGenericType)
                throw new CompositionException("Requested contract " + contractType.Name + " is not a generic type.");

            if (contractType.ContainsGenericParameters)
                throw new CompositionException("Requested contract " + contractType.Name +
                                               " is not fully constructed and closed.");

            return _closedContractToComponentMap.GetOrAdd(contractType, closedContractType =>
            {
                var genericContractType = closedContractType.GetGenericTypeDefinition();
                if (!_openToBoundContractTypeMap.ContainsKey(genericContractType))
                    return null;

                var originalGenericContractType = _openToBoundContractTypeMap[genericContractType];
                return CloseGenericType(TargetType, originalGenericContractType, closedContractType);
            });
        }
        
        private Type CloseGenericType(Type openType, Type templateType, Type closedType)
        {
            var templateTypeParams = templateType.GetGenericArguments();
            var closedTypeParams = closedType.GetGenericArguments();

            if (templateTypeParams.Length != closedTypeParams.Length)
                return null;
            
            // If there is a non-generic type parameter in the template that does not match the corresponding requested
            // closed type parameter, we can't build a compatible type. (Because even if all generic parameters are
            // replaced with correct ones, the type parameters won't match.)
            if (templateTypeParams.Where((t, i) => !t.IsGenericParameter && t != closedTypeParams[i]).Any())
                return null;
            
            var currentType = openType;

            while (currentType.ContainsGenericParameters)
            {
                Type[] currentTypeParams = currentType.GetGenericArguments();

                int currentTypeParamIndex = -1;

                for (int i = 0; i < currentTypeParams.Length; i++)
                    if (currentTypeParams[i].IsGenericParameter)
                        currentTypeParamIndex = i;

                if (currentTypeParamIndex < 0)
                    return null;

                Type currentTypeParam = currentTypeParams[currentTypeParamIndex];

                int closedTypeParamIndex = -1;
                for (int i = 0; i < templateTypeParams.Length; i++)
                    if (templateTypeParams[i] == currentTypeParam)
                        closedTypeParamIndex = i;

                if (closedTypeParamIndex < 0)
                {
                    // Attempt to match by name, in case the generic contract is provided externally
                    // (eg. Fluent interface)
                    
                    for (int i = 0; i < templateTypeParams.Length; i++)
                        if (templateTypeParams[i].Name == currentTypeParam.Name)
                            closedTypeParamIndex = i;
                }

                if (closedTypeParamIndex < 0)
                {
                    // Neither found by type matching nor name
                    return null;
                }

                Type closedTypeParam = closedTypeParams[closedTypeParamIndex];

                currentTypeParams[currentTypeParamIndex] = closedTypeParam;

                currentType = currentType
                    .GetGenericTypeDefinition()
                    .MakeGenericType(currentTypeParams);
            }

            // Make sure the closed type is assignable to the requested closed type
            // (This might not be true for recurring type parameters in declaration)
            return closedType.IsAssignableFrom(currentType) ? currentType : null;
        }

    }
}