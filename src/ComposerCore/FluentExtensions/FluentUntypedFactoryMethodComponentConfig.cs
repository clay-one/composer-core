using System;
using System.Collections.Generic;
using ComposerCore.Cache;
using ComposerCore.CompositionalQueries;
using ComposerCore.Factories;
using ComposerCore.Implementation;

namespace ComposerCore.FluentExtensions
{
    public class FluentUntypedFactoryMethodComponentConfig
    {
        protected readonly ComponentContext Context;
        protected readonly UntypedFactoryMethodComponentFactory Factory;

        #region Constructors

        public FluentUntypedFactoryMethodComponentConfig(ComponentContext context, Func<IComposer, object> factoryMethod)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Factory = new UntypedFactoryMethodComponentFactory(factoryMethod);
        }

        #endregion

        #region Fluent configuration methods

        public void RegisterWith<TContract>(string contractName = null)
        {
            RegisterWith(typeof(TContract), contractName);
        }

        public void RegisterWith(Type contractType, string contractName = null)
        {
            Factory.ContractTypes = new List<Type> { contractType };
            Context.Register(contractType, contractName, Factory);
        }

        public FluentUntypedFactoryMethodComponentConfig UseComponentCache(Type cacheContractType, string cacheContractName = null)
        {
            if (cacheContractType == null)
                Factory.ComponentCacheQuery = new NullQuery();
            else
                Factory.ComponentCacheQuery = new ComponentQuery(cacheContractType, cacheContractName);

            return this;
        }

        public FluentUntypedFactoryMethodComponentConfig UseComponentCache<TCacheContract>(string cacheContractName = null)
        {
            return UseComponentCache(typeof(TCacheContract), cacheContractName);
        }

        public FluentUntypedFactoryMethodComponentConfig AsSingleton()
        {
            return UseComponentCache(typeof(ContractAgnosticComponentCache));
        }

        public FluentUntypedFactoryMethodComponentConfig AsTransient()
        {
            return UseComponentCache(null);
        }


        #endregion
    }
}