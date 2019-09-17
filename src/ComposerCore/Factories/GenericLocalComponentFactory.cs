using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
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
        private readonly List<InitializationPointSpecification> _initializationPoints;

        public GenericLocalComponentFactory(Type targetType)
        {
            if ((!targetType.ContainsGenericParameters) || (!targetType.IsGenericType))
                throw new ArgumentException("TargetType in GenericLocalComponentFactory should be an open generic type.");

            _targetType = targetType;
            _composer = null;

            _contractTypes = new Dictionary<Type, Type>();
            _subFactories = new ConcurrentDictionary<Type, LocalComponentFactory>();
            _initializationPoints = new List<InitializationPointSpecification>();

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

            var subFactory = _subFactories.GetOrAdd(closedTargetType, type =>
            {
                var newSubFactory = new LocalComponentFactory(type);
                newSubFactory.InitializationPoints.AddRange(_initializationPoints);
                newSubFactory.Initialize(_composer);
                return newSubFactory;
            });

            return subFactory.GetComponentInstance(contract, listenerChain);
        }

        #endregion

        #region Public methods and properties

        public List<InitializationPointSpecification> InitializationPoints
        {
            get
            {
                if (_composer != null)
                    throw new InvalidOperationException("Cannot access InitializationPoints when the factory is initialized.");

                return _initializationPoints;
            }
        }

        public void AddOpenGenericContract(Type openContractType)
        {
            if (!openContractType.ContainsGenericParameters || !openContractType.IsGenericType)
            {
                throw new ArgumentException($"The contract type {openContractType.FullName} is not an open generic type.");
            }

            var genericTypeDefinition = openContractType.GetGenericTypeDefinition();
            if (!_contractTypes.ContainsKey(genericTypeDefinition))
                _contractTypes.Add(genericTypeDefinition, openContractType);
        }

        #endregion

        #region Private helper methods

        private void ExtractContractTypes()
        {
            var openContracts = ComponentContextUtils.FindContracts(_targetType)
                .Where(t => t.ContainsGenericParameters && t.IsGenericType);

            foreach (var openContract in openContracts)
            {
                _contractTypes.Add(openContract.GetGenericTypeDefinition(), openContract);
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