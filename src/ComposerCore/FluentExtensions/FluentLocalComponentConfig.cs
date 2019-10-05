using System;
using System.Reflection;
using ComposerCore.Attributes;
using ComposerCore.Cache;
using ComposerCore.CompositionalQueries;
using ComposerCore.Factories;
using ComposerCore.Implementation;

namespace ComposerCore.FluentExtensions
{
    public class FluentLocalComponentConfig
    {
        protected readonly ComponentContext Context;
        protected readonly LocalComponentFactory Factory;

        #region Constructors

        public FluentLocalComponentConfig(ComponentContext context, LocalComponentFactory factory)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Factory = factory ?? throw new ArgumentNullException(nameof(factory));
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

        public void RegisterAsItself(string contractName = null)
        {
            RegisterWith(Factory.TargetType, contractName);
        }

        public FluentLocalComponentConfig SetComponent<TPlugContract>(
            string memberName, string contractName = null, bool? required = null)
        {
            return SetComponent(memberName, typeof(TPlugContract), contractName, required);
        }

        public FluentLocalComponentConfig SetComponent(
            string memberName, Type contractType, string contractName = null, bool? required = null)
        {
            Factory.InitializationPoints.Add(new InitializationPointSpecification(memberName, MemberTypes.All,
                required, new ComponentQuery(contractType, contractName)));

            return this;
        }

        public FluentLocalComponentConfig UseConstructor(params Type[] argTypes)
        {
            foreach (var argType in argTypes)
            {
                AddConstructorComponent(argType);
            }
            
            return this;
        }

        public FluentLocalComponentConfig AddConstructorComponent<TPlugContract>(string contractName = null, bool? required = null)
        {
            return AddConstructorComponent(typeof(TPlugContract), contractName, required);
        }

        public FluentLocalComponentConfig AddConstructorComponent(Type contractType, string contractName = null, bool? required = null)
        {
            Factory.ConstructorArgs.Add(new ConstructorArgSpecification(required, new ComponentQuery(contractType, contractName)));
            return this;
        }

        public FluentLocalComponentConfig AddConstructorValue(object value)
        {
            Factory.ConstructorArgs.Add(new ConstructorArgSpecification(false, new SimpleValueQuery(value)));
            return this;
        }

        public FluentLocalComponentConfig AddConstructorValue<TMember>(Func<IComposer, TMember> valueCalculator, bool? required = null)
        {
            Factory.ConstructorArgs.Add(new ConstructorArgSpecification(required, 
                new FuncValueQuery(c => valueCalculator(c))));

            return this;
        }

        public FluentLocalComponentConfig AddConstructorValueFromVariable(string variableName, bool? required = null)
        {
            Factory.ConstructorArgs.Add(new ConstructorArgSpecification(required, new VariableQuery(variableName)));
            return this;
        }

        public FluentLocalComponentConfig SetValue(string memberName, object value)
        {
            Factory.InitializationPoints.Add(new InitializationPointSpecification(memberName, MemberTypes.All, false, 
                new SimpleValueQuery(value)));

            return this;
        }

        public FluentLocalComponentConfig SetValue<TMember>(string memberName, Func<IComposer, TMember> valueCalculator, bool? required = null)
        {
            Factory.InitializationPoints.Add(new InitializationPointSpecification(memberName, MemberTypes.All, required,
                new FuncValueQuery(c => valueCalculator(c))));

            return this;
        }

        public FluentLocalComponentConfig SetValueFromVariable(string memberName, string variableName, bool? required = null)
        {
            Factory.InitializationPoints.Add(new InitializationPointSpecification(memberName, MemberTypes.All, required,
                new VariableQuery(variableName)));

            return this;
        }

        public FluentLocalComponentConfig NotifyInitialized(Action<IComposer, object> initAction)
        {
            Factory.CompositionNotificationMethods.Add(initAction);
            return this;
        }

        public FluentLocalComponentConfig UseComponentCache(Type cacheContractType, string cacheContractName = null)
        {
            if (cacheContractType == null)
                Factory.ComponentCacheQuery = new NullQuery();
            else
                Factory.ComponentCacheQuery = new ComponentQuery(cacheContractType, cacheContractName);

            return this;
        }

        public FluentLocalComponentConfig UseComponentCache<TCacheContract>(string cacheContractName = null)
        {
            return UseComponentCache(typeof(TCacheContract), cacheContractName);
        }

        public FluentLocalComponentConfig AsSingleton()
        {
            return UseComponentCache(typeof(ContractAgnosticComponentCache));
        }

        public FluentLocalComponentConfig AsTransient()
        {
            return UseComponentCache(null);
        }

        public FluentLocalComponentConfig SetConstructorResolutionPolicy(ConstructorResolutionPolicy policy)
        {
//            Factory.ConstructorResolutionPolicy = policy;
//            return this;

            throw new NotImplementedException();
        }
        
        #endregion
    }
}