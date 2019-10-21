using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using ComposerCore.Extensibility;
using ComposerCore.Factories;

namespace ComposerCore.Implementation
{
    public class GenericComponentRegistration : IComponentRegistration
    {
        /// <summary>
        /// Generic type definition (obtained by calling ) 
        /// -> 
        /// Constructed generic type (obtained from base classes and interfaces)
        /// </summary>
        private readonly IDictionary<Type, Type> _contractTypes;

        private readonly ConcurrentDictionary<Type, Type> _closedContractToComponentMap;
        private readonly ConcurrentDictionary<Type, LocalComponentFactory> _subFactories;
        
        public GenericComponentRegistration(IComponentFactory factory) // : base(factory)
        {
//            if (!targetType.IsOpenGenericType())
//                throw new ArgumentException("TargetType in GenericLocalComponentFactory should be an open generic type.");
//
//            _contractTypes = new Dictionary<Type, Type>();
//            _closedContractToComponentMap = new ConcurrentDictionary<Type, Type>();
//            _subFactories = new ConcurrentDictionary<Type, LocalComponentFactory>();
//
//            ExtractContractTypes();
        }
        
        public bool IsResolvable(Type contractType)
        {
            var requestedClosedContractType = contractType;
            if (!requestedClosedContractType.IsGenericType || requestedClosedContractType.ContainsGenericParameters)
                return false;

            return MapToClosedComponentType(contractType) != null;
        }

        public object GetComponent(ContractIdentity identity, IComposer dependencyResolver)
        {
            throw new NotImplementedException();
        }

        private Type MapToClosedComponentType(Type contractType)
        {
            throw new NotImplementedException();
        }
    }
}