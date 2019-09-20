using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ComposerCore.Extensibility;
using ComposerCore.Implementation;
using ComposerCore.Utility;

namespace ComposerCore.Factories
{
    public class GenericLocalComponentFactory : ILocalComponentFactory
    {
        private IComposer _composer;

        /// <summary>
        /// Generic type definition (obtained by calling ) 
        /// -> 
        /// Constructed generic type (obtained from base classes and interfaces)
        /// </summary>
        private readonly IDictionary<Type, Type> _contractTypes;

        private readonly ConcurrentDictionary<Type, LocalComponentFactory> _subFactories;
        private readonly Type _targetType;
        
        private List<ConstructorArgSpecification> _constructorArgs;
        private readonly List<InitializationPointSpecification> _initializationPoints;
        private List<Action<IComposer, object>> _compositionNotificationMethods;
        private ICompositionalQuery _componentCacheQuery;

        public GenericLocalComponentFactory(Type targetType)
        {
            if (!targetType.IsOpenGenericType())
                throw new ArgumentException("TargetType in GenericLocalComponentFactory should be an open generic type.");

            _targetType = targetType;
            _composer = null;

            _contractTypes = new Dictionary<Type, Type>();
            _subFactories = new ConcurrentDictionary<Type, LocalComponentFactory>();
            _constructorArgs = new List<ConstructorArgSpecification>();
            _initializationPoints = new List<InitializationPointSpecification>();
            _compositionNotificationMethods = new List<Action<IComposer, object>>();

            ExtractContractTypes();
        }

        #region Implementation of IComponentFactory

        public bool ValidateContractType(Type contract)
        {
            return _targetType.IsAssignableToGenericType(contract);
        }

        public void Initialize(IComposer composer)
        {
            if (_contractTypes.Count < 1)
                throw new CompositionException($"No open contracts found nor added for the type {_targetType.Name}. " +
                                               "Use [Contract] attribute or use Fluent syntax to introduce contracts");
            
            _composer = composer;
        }

        public IEnumerable<Type> GetContractTypes()
        {
            return _contractTypes.Keys;
        }

        public object GetComponentInstance(ContractIdentity contract, IEnumerable<ICompositionListener> listenerChain)
        {
            var requestedClosedContractType = contract.Type;

            if (!requestedClosedContractType.IsGenericType)
                throw new CompositionException("Requested contract " + requestedClosedContractType.Name + " is not a generic type.");

            if (requestedClosedContractType.ContainsGenericParameters)
                throw new CompositionException("Requested contract " + requestedClosedContractType.Name +
                                               " is not fully constructed and closed.");

            var requestedGenericContractType = contract.Type.GetGenericTypeDefinition();
            if (!_contractTypes.ContainsKey(requestedGenericContractType))
                throw new CompositionException("The requested generic contract type definition " +
                                               requestedGenericContractType.Name +
                                               " is not among the supported contracts for this factory.");

            var originalGenericContractType = _contractTypes[requestedGenericContractType];
            var closedTargetType = CloseGenericType(_targetType, originalGenericContractType, requestedClosedContractType);

            if (closedTargetType == null)
            {
                throw new CompositionException("Failed to construct a closed generic type.\n" +
                                               "Please provide the following information when you register an issue:\n" +
                                               $"Component's open generic type: {_targetType.FullName}\n" +
                                               $"Requested closed contract type: {requestedClosedContractType.FullName}\n" +
                                               $"Requested open contract type: {requestedGenericContractType.FullName}\n" +
                                               $"Original generic contract type: {originalGenericContractType.FullName}");
            }

            var subFactory = _subFactories.GetOrAdd(closedTargetType, type =>
            {
                var newSubFactory = new LocalComponentFactory(type);
                
                newSubFactory.ConstructorArgs.AddRange(_constructorArgs);
                newSubFactory.InitializationPoints.AddRange(_initializationPoints);
                newSubFactory.ComponentCacheQuery = _componentCacheQuery;
                newSubFactory.CompositionNotificationMethods.AddRange(_compositionNotificationMethods);
                newSubFactory.Initialize(_composer);
                return newSubFactory;
            });

            return subFactory.GetComponentInstance(contract, listenerChain);
        }

        #endregion

        #region Public methods and properties
        
        public List<ConstructorArgSpecification> ConstructorArgs
        {
            get
            {
                if (_composer != null)
                    throw new InvalidOperationException("Cannot access ConstructorArgs when the factory is initialized.");

                return _constructorArgs ?? (_constructorArgs = new List<ConstructorArgSpecification>());
            }
        }

        public List<InitializationPointSpecification> InitializationPoints
        {
            get
            {
                if (_composer != null)
                    throw new InvalidOperationException("Cannot access InitializationPoints when the factory is initialized.");

                return _initializationPoints;
            }
        }
        
        public List<Action<IComposer, object>> CompositionNotificationMethods
        {
            get
            {
                if (_composer != null)
                    throw new InvalidOperationException("Cannot access CompositionNotificationMethods when the factory is initialized.");

                return _compositionNotificationMethods ?? (_compositionNotificationMethods = new List<Action<IComposer, object>>());
            }
        }

        public ICompositionalQuery ComponentCacheQuery
        {
            get
            {
                if (_composer != null)
                    throw new InvalidOperationException("Cannot access ComponentCacheQuery when the factory is initialized.");

                return _componentCacheQuery;
            }
            set
            {
                if (_composer != null)
                    throw new InvalidOperationException("Cannot access ComponentCacheQuery when the factory is initialized.");

                _componentCacheQuery = value;
            }
        }


        public void AddOpenGenericContract(Type openContractType)
        {
            if (!openContractType.IsOpenGenericType())
            {
                throw new ArgumentException($"The contract type {openContractType.FullName} is not an open generic type.");
            }

            var boundGenericContract = _targetType
                .GetBaseTypes(true)
                .Concat(_targetType.GetInterfaces())
                .FirstOrDefault(t => t.IsGenericType && t.GetGenericTypeDefinition() == openContractType);

            if (boundGenericContract == null)
                throw new CompositionException($"The open contract type {openContractType.FullName} could not be" +
                                               $"found in the hierarchy of tar get type {_targetType.FullName}");

            var unresolvableGenericArgs = 
                _targetType.GetGenericArguments().Except(boundGenericContract.GetGenericArguments()).ToArray();
            if (unresolvableGenericArgs.Any())
            {
                var unresolvableGenericArgsString =
                    string.Join(";\n", unresolvableGenericArgs.Select(a => a.ToString()));
                throw new CompositionException($"The contract type {openContractType.FullName} does not contain " +
                                               "enough type arguments to allow constructing a completely closed " +
                                               $"component type out of {_targetType.FullName}. The missing type " +
                                               $"arguments are:\n{unresolvableGenericArgsString}");
            }

            if (!_contractTypes.ContainsKey(openContractType))
                _contractTypes.Add(openContractType, boundGenericContract);
        }

        #endregion

        #region Private helper methods

        private void ExtractContractTypes()
        {
            var boundGenericContracts = ComponentContextUtils.FindContracts(_targetType)
                .Where(t => t.IsOpenGenericType());

            foreach (var boundGenericContract in boundGenericContracts)
            {
                var openContract = boundGenericContract.GetGenericTypeDefinition();
                _contractTypes.Add(openContract, boundGenericContract);
            }
        }

        private Type CloseGenericType(Type openType, Type templateType, Type closedType)
        {
            var templateTypeParams = templateType.GetGenericArguments();
            var closedTypeParams = closedType.GetGenericArguments();

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

            return currentType;
        }

        #endregion
    }
}