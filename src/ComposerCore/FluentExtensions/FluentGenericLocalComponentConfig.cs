using System;
using ComposerCore.Attributes;
using ComposerCore.CompositionalQueries;
using ComposerCore.Extensibility;
using ComposerCore.Implementation;

namespace ComposerCore.FluentExtensions
{
    public class FluentGenericLocalComponentConfig : FluentComponentConfigBase<FluentGenericLocalComponentConfig>
    {
        private readonly GenericComponentRegistration _genericComponentRegistration;

        protected override IComponentRegistration Registration => _genericComponentRegistration;

        #region Constructors

        public FluentGenericLocalComponentConfig(ComponentContext context, GenericComponentRegistration registration)
            : base(context)
        {
            _genericComponentRegistration = registration;
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
            _genericComponentRegistration.AddConfiguredInitializationPoint(new ComponentQuery(contractType, contractName), memberName,
                null, required);

            return this;
        }

        public FluentGenericLocalComponentConfig AddConstructorComponent<TPlugContract>(string contractName = null, bool required = true)
        {
            return AddConstructorComponent(typeof(TPlugContract), contractName, required);
        }

        public FluentGenericLocalComponentConfig AddConstructorComponent(Type contractType, string contractName = null, bool required = true)
        {
            _genericComponentRegistration.AddConfiguredConstructorArg(new ComponentQuery(contractType, contractName), required);
            return this;
        }

        public FluentGenericLocalComponentConfig AddConstructorValue(object value)
        {
            _genericComponentRegistration.AddConfiguredConstructorArg(new SimpleValueQuery(value), false);
            return this;
        }

        public FluentGenericLocalComponentConfig AddConstructorValue<TMember>(Func<IComposer, TMember> valueCalculator, bool required = true)
        {
            _genericComponentRegistration.AddConfiguredConstructorArg(new FuncValueQuery(c => valueCalculator(c)), required);
            return this;
        }

        public FluentGenericLocalComponentConfig AddConstructorValueFromVariable(string variableName, bool required = true)
        {
            _genericComponentRegistration.AddConfiguredConstructorArg(new VariableQuery(variableName), required);
            return this;
        }

        public FluentGenericLocalComponentConfig SetValue(string memberName, object value)
        {
            _genericComponentRegistration.AddConfiguredInitializationPoint(new SimpleValueQuery(value), memberName);
            return this;
        }

        public FluentGenericLocalComponentConfig SetValue<TMember>(string memberName, Func<IComposer, TMember> valueCalculator, bool? required = null)
        {
            _genericComponentRegistration.AddConfiguredInitializationPoint(new FuncValueQuery(c => valueCalculator(c)), memberName, null, required);
            return this;
        }

        public FluentGenericLocalComponentConfig SetValueFromVariable(string memberName, string variableName, bool? required = null)
        {
            _genericComponentRegistration.AddConfiguredInitializationPoint(new VariableQuery(variableName), memberName, null, required);
            return this;
        }

        public FluentGenericLocalComponentConfig NotifyInitialized(Action<IComposer, object> initAction)
        {
            _genericComponentRegistration.AddCompositionNotification(initAction);
            return this;
        }
        
        public FluentGenericLocalComponentConfig SetConstructorResolutionPolicy(ConstructorResolutionPolicy policy)
        {
            _genericComponentRegistration.SetConstructorResolutionPolicy(policy);
            return this;
        }

        #endregion
    }
}