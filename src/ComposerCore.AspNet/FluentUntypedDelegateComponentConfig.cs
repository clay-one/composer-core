using System;
using System.Collections.Generic;
using ComposerCore.CompositionalQueries;
using ComposerCore.Implementation;

namespace ComposerCore.Factories
{
    public class FluentUntypedDelegateComponentConfig
    {
        protected readonly ComponentContext Context;
        protected readonly UntypedDelegateComponentFactory Factory;

        #region Constructors

        public FluentUntypedDelegateComponentConfig(ComponentContext context, Func<IComposer, object> factoryMethod)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Factory = new UntypedDelegateComponentFactory(factoryMethod);
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

        public FluentUntypedDelegateComponentConfig UseComponentCache(Type cacheContractType, string cacheContractName = null)
        {
            if (cacheContractType == null)
                Factory.ComponentCacheQuery = new NullQuery();
            else
                Factory.ComponentCacheQuery = new ComponentQuery(cacheContractType, cacheContractName);

            return this;
        }

        public FluentUntypedDelegateComponentConfig UseComponentCache<TCacheContract>(string cacheContractName = null)
        {
            return UseComponentCache(typeof(TCacheContract), cacheContractName);
        }

        #endregion
    }
}