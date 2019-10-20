using System;
using System.Reflection;
using ComposerCore.Attributes;
using ComposerCore.CompositionalQueries;
using ComposerCore.Factories;
using ComposerCore.Implementation;

namespace ComposerCore.FluentExtensions
{
    public class FluentGenericLocalComponentConfig : FluentComponentConfigBase<FluentGenericLocalComponentConfig>
    {
        protected readonly GenericLocalComponentFactory Factory;

        #region Constructors

        public FluentGenericLocalComponentConfig(ComponentContext context, GenericLocalComponentFactory factory)
            : base(context)
        {
            Factory = factory ?? throw new ArgumentNullException(nameof(factory));
            Registration = new ComponentRegistration(Factory);
        }

        #endregion
        
        #region Fluent configuration methods

        public FluentGenericLocalComponentConfig SetComponent<TPlugContract>(
            string memberName, string contractName = null, bool required = true)
        {
            return SetComponent(memberName, typeof(TPlugContract), contractName, required);
        }

        public FluentGenericLocalComponentConfig SetComponent(
            string memberName, Type contractType, string contractName = null, bool? required = null)
        {
            Factory.InitializationPoints.Add(new InitializationPointSpecification(memberName, MemberTypes.All,
                required, new ComponentQuery(contractType, contractName)));

            return this;
        }

        public FluentGenericLocalComponentConfig AddConstructorComponent<TPlugContract>(string contractName = null, bool required = true)
        {
            return AddConstructorComponent(typeof(TPlugContract), contractName, required);
        }

        public FluentGenericLocalComponentConfig AddConstructorComponent(Type contractType, string contractName = null, bool required = true)
        {
            Factory.AddConfiguredConstructorArg(new ConstructorArgSpecification(required, new ComponentQuery(contractType, contractName)));
            return this;
        }

        public FluentGenericLocalComponentConfig AddConstructorValue(object value)
        {
            Factory.AddConfiguredConstructorArg(new ConstructorArgSpecification(false, new SimpleValueQuery(value)));
            return this;
        }

        public FluentGenericLocalComponentConfig AddConstructorValue<TMember>(Func<IComposer, TMember> valueCalculator, bool required = true)
        {
            Factory.AddConfiguredConstructorArg(new ConstructorArgSpecification(required, 
                new FuncValueQuery(c => valueCalculator(c))));

            return this;
        }

        public FluentGenericLocalComponentConfig AddConstructorValueFromVariable(string variableName, bool required = true)
        {
            Factory.AddConfiguredConstructorArg(new ConstructorArgSpecification(required, new VariableQuery(variableName)));
            return this;
        }

        public FluentGenericLocalComponentConfig SetValue(string memberName, object value)
        {
            Factory.InitializationPoints.Add(new InitializationPointSpecification(memberName, MemberTypes.All, false, 
                new SimpleValueQuery(value)));

            return this;
        }

        public FluentGenericLocalComponentConfig SetValue<TMember>(string memberName, Func<IComposer, TMember> valueCalculator, bool? required = null)
        {
            Factory.InitializationPoints.Add(new InitializationPointSpecification(memberName, MemberTypes.All, required,
                new FuncValueQuery(c => valueCalculator(c))));

            return this;
        }

        public FluentGenericLocalComponentConfig SetValueFromVariable(string memberName, string variableName, bool? required = null)
        {
            Factory.InitializationPoints.Add(new InitializationPointSpecification(memberName, MemberTypes.All, required,
                new VariableQuery(variableName)));

            return this;
        }

        public FluentGenericLocalComponentConfig NotifyInitialized(Action<IComposer, object> initAction)
        {
            Factory.CompositionNotificationMethods.Add(initAction);
            return this;
        }
        
        public FluentGenericLocalComponentConfig SetConstructorResolutionPolicy(ConstructorResolutionPolicy policy)
        {
            Factory.ConstructorResolutionPolicy = policy;
            return this;
        }

        #endregion
    }
}