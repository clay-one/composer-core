using System;
using System.Reflection;
using ComposerCore.CompositionalQueries;
using ComposerCore.Factories;
using ComposerCore.Implementation;

namespace ComposerCore.FluentExtensions
{
    public class FluentGenericLocalComponentConfig
    {
        protected readonly ComponentContext Context;
        protected readonly GenericLocalComponentFactory Factory;

        #region Constructors

        public FluentGenericLocalComponentConfig(ComponentContext context, GenericLocalComponentFactory factory)
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
            if (contractType.ContainsGenericParameters && contractType.IsGenericType)
                Factory.AddOpenGenericContract(contractType);
            
            Context.Register(contractType, contractName, Factory);
        }

        public FluentGenericLocalComponentConfig SetComponent<TPlugContract>(
            string memberName, string contractName = null, bool required = true)
        {
            return SetComponent(memberName, typeof(TPlugContract), contractName, required);
        }

        public FluentGenericLocalComponentConfig SetComponent(
            string memberName, Type contractType, string contractName = null, bool required = true)
        {
            Factory.InitializationPoints.Add(new InitializationPointSpecification(memberName, MemberTypes.All,
                required, new ComponentQuery(contractType, contractName)));

            return this;
        }

//        public FluentGenericLocalComponentConfig UseConstructor(params Type[] argTypes)
//        {
//            Factory.TargetConstructor = Factory.TargetType.GetConstructor(argTypes);
//            return this;
//        }
//
//        public FluentGenericLocalComponentConfig AddConstructorComponent<TPlugContract>(string contractName = null, bool required = true)
//        {
//            return AddConstructorComponent(typeof(TPlugContract), contractName, required);
//        }
//
//        public FluentGenericLocalComponentConfig AddConstructorComponent(Type contractType, string contractName = null, bool required = true)
//        {
//            Factory.ConstructorArgs.Add(new ConstructorArgSpecification(required, new ComponentQuery(contractType, contractName)));
//            return this;
//        }
//
//        public FluentGenericLocalComponentConfig AddConstructorValue(object value)
//        {
//            Factory.ConstructorArgs.Add(new ConstructorArgSpecification(false, new SimpleValueQuery(value)));
//            return this;
//        }
//
//        public FluentGenericLocalComponentConfig AddConstructorValue<TMember>(Func<IComposer, TMember> valueCalculator, bool required = true)
//        {
//            Factory.ConstructorArgs.Add(new ConstructorArgSpecification(required, 
//                new FuncValueQuery(c => valueCalculator(c))));
//
//            return this;
//        }
//
//        public FluentGenericLocalComponentConfig AddConstructorValueFromVariable(string variableName, bool required = true)
//        {
//            Factory.ConstructorArgs.Add(new ConstructorArgSpecification(required, new VariableQuery(variableName)));
//            return this;
//        }
//
        public FluentGenericLocalComponentConfig SetValue(string memberName, object value)
        {
            Factory.InitializationPoints.Add(new InitializationPointSpecification(memberName, MemberTypes.All, false, 
                new SimpleValueQuery(value)));

            return this;
        }

        public FluentGenericLocalComponentConfig SetValue<TMember>(string memberName, Func<IComposer, TMember> valueCalculator, bool required = true)
        {
            Factory.InitializationPoints.Add(new InitializationPointSpecification(memberName, MemberTypes.All, required,
                new FuncValueQuery(c => valueCalculator(c))));

            return this;
        }

        public FluentGenericLocalComponentConfig SetValueFromVariable(string memberName, string variableName, bool required = true)
        {
            Factory.InitializationPoints.Add(new InitializationPointSpecification(memberName, MemberTypes.All, required,
                new VariableQuery(variableName)));

            return this;
        }

//        public FluentGenericLocalComponentConfig NotifyInitialized(Action<IComposer, object> initAction)
//        {
//            Factory.CompositionNotificationMethods.Add(initAction);
//            return this;
//        }
//
//        public FluentGenericLocalComponentConfig UseComponentCache(Type cacheContractType, string cacheContractName = null)
//        {
//            if (cacheContractType == null)
//                Factory.ComponentCacheQuery = new NullQuery();
//            else
//                Factory.ComponentCacheQuery = new ComponentQuery(cacheContractType, cacheContractName);
//
//            return this;
//        }
//
//        public FluentGenericLocalComponentConfig UseComponentCache<TCacheContract>(string cacheContractName = null)
//        {
//            return UseComponentCache(typeof(TCacheContract), cacheContractName);
//        }
//
        #endregion
    }
}