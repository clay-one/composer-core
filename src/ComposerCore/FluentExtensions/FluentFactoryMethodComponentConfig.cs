using System;
using System.Collections.Generic;
using ComposerCore.Cache;
using ComposerCore.CompositionalQueries;
using ComposerCore.Factories;
using ComposerCore.Implementation;

namespace ComposerCore.FluentExtensions
{
    public class FluentFactoryMethodComponentConfig<TComponent> where TComponent : class
    {
        protected readonly ComponentContext Context;
        protected readonly FactoryMethodComponentFactory<TComponent> Factory;

        #region Constructors

        public FluentFactoryMethodComponentConfig(ComponentContext context, Func<IComposer, TComponent> factoryMethod)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Factory = new FactoryMethodComponentFactory<TComponent>(factoryMethod);
        }

        #endregion
        
        #region Fluent configuration methods

        public void Register(string contractName = null)
        {
            Context.Register(contractName, Factory);
        }
        
        public void RegisterWith<TContract>(string contractName = null)
        {
            RegisterWith(typeof(TContract), contractName);
        }

        public void RegisterWith(Type contractType, string contractName = null)
        {
            Context.Register(contractType, contractName, Factory);
        }

        public FluentFactoryMethodComponentConfig<TComponent> UseComponentCache(Type cacheContractType, string cacheContractName = null)
        {
            throw new NotImplementedException();
//            if (cacheContractType == null)
//                Factory.ComponentCacheQuery = new NullQuery();
//            else
//                Factory.ComponentCacheQuery = new ComponentQuery(cacheContractType, cacheContractName);

//            return this;
        }

        public FluentFactoryMethodComponentConfig<TComponent> UseComponentCache<TCacheContract>(string cacheContractName = null)
        {
            return UseComponentCache(typeof(TCacheContract), cacheContractName);
        }
        
        public FluentFactoryMethodComponentConfig<TComponent> AsSingleton()
        {
            return UseComponentCache(typeof(ContractAgnosticComponentCache));
        }

        public FluentFactoryMethodComponentConfig<TComponent> AsTransient()
        {
            return UseComponentCache(null);
        }
        
        #endregion

    }
}