using System;
using ComposerCore.Cache;
using ComposerCore.Extensibility;
using ComposerCore.Implementation;

namespace ComposerCore.FluentExtensions
{
    public abstract class FluentComponentConfigBase<TSubType> where TSubType : class
    {
        protected readonly ComponentContext Context;
        protected abstract IComponentRegistration Registration { get; }

        protected FluentComponentConfigBase(ComponentContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }
        
        #region Terminating methods

        public void Register()
        {
            Context.Register(Registration);
        }
        
        public void Register(string contractName)
        {
            Registration.SetDefaultContractName(contractName);
            Context.Register(Registration);
        }
        
        public void RegisterWith<TContract>()
        {
            RegisterWith(typeof(TContract));
        }

        public void RegisterWith<TContract>(string contractName)
        {
            RegisterWith(typeof(TContract), contractName);
        }

        public void RegisterWith(Type contractType)
        {
            Registration.AddContractType(contractType);
            Context.Register(Registration);
        }

        public void RegisterWith(Type contractType, string contractName)
        {
            Registration.AddContract(new ContractIdentity(contractType, contractName));
            Context.Register(Registration);
        }

        #endregion
        
        #region Fluent configuration methods

        public TSubType UseComponentCache(Type cacheContractType)
        {
            Registration.SetCache(cacheContractType == null ? nameof(NoComponentCache) : cacheContractType.Name);
            return this as TSubType;
        }

        public TSubType UseComponentCache<TCacheContract>()
        {
            return UseComponentCache(typeof(TCacheContract));
        }
        
        public TSubType AsSingleton()
        {
            return UseComponentCache(typeof(ContractAgnosticComponentCache));
        }

        public TSubType AsScoped()
        {
            return UseComponentCache(typeof(ScopedComponentCache));
        }

        public TSubType AsTransient()
        {
            return UseComponentCache(null);
        }
        
        #endregion

    }
}