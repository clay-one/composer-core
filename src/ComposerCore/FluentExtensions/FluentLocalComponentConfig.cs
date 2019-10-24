using System;
using System.Reflection;
using ComposerCore.Attributes;
using ComposerCore.CompositionalQueries;
using ComposerCore.Extensibility;
using ComposerCore.Implementation;

namespace ComposerCore.FluentExtensions
{
    public class FluentLocalComponentConfig : FluentComponentConfigBase<FluentLocalComponentConfig>
    {
        protected readonly ConcreteTypeRegistration _concreteTypeRegistration;

        protected override IComponentRegistration Registration => _concreteTypeRegistration;

        #region Constructors

        public FluentLocalComponentConfig(ComponentContext context, Type targetType)
            : base(context)
        {
            _concreteTypeRegistration = new ConcreteTypeRegistration(targetType ?? throw new ArgumentNullException(nameof(targetType)));
        }

        #endregion
        
        #region Terminating methods
        
        public void RegisterAsItself()
        {
            RegisterWith(_concreteTypeRegistration.TargetType);
        }

        public void RegisterAsItself(string contractName)
        {
            RegisterWith(_concreteTypeRegistration.TargetType, contractName);
        }

        #endregion

        #region Fluent configuration methods

        public FluentLocalComponentConfig SetComponent<TPlugContract>(
            string memberName, string contractName = null, bool? required = null)
        {
            return SetComponent(memberName, typeof(TPlugContract), contractName, required);
        }

        public FluentLocalComponentConfig SetComponent(
            string memberName, Type contractType, string contractName = null, bool? required = null)
        {
            _concreteTypeRegistration.AddConfiguredInitializationPoint(
                new ComponentQuery(contractType, contractName), memberName, null, required);

            return this;
        }

        public FluentLocalComponentConfig UseConstructor(params Type[] argTypes)
        {
            var constructor = _concreteTypeRegistration.TargetType.GetConstructor(argTypes);
            if (constructor == null)
                throw new ArgumentException("Could not find a public constructor with the specified parameter types " +
                                            $"in the class '{_concreteTypeRegistration.TargetType}'");

            return UseConstructor(constructor);
        }

        public FluentLocalComponentConfig UseConstructor(ConstructorInfo constructorInfo)
        {
            if (constructorInfo == null)
                throw new ArgumentNullException(nameof(constructorInfo));
            
            Context.GetComponent<IPresetConstructorStore>().SetConstructor(_concreteTypeRegistration.TargetType, constructorInfo);
            _concreteTypeRegistration.SetConstructorResolutionPolicy(ConstructorResolutionPolicy.Preset);
            
            return this;
        }

        public FluentLocalComponentConfig AddConstructorComponent<TPlugContract>(string contractName = null, bool? required = null)
        {
            return AddConstructorComponent(typeof(TPlugContract), contractName, required);
        }

        public FluentLocalComponentConfig AddConstructorComponent(Type contractType, string contractName = null, bool? required = null)
        {
            _concreteTypeRegistration.AddConfiguredConstructorArg(new ComponentQuery(contractType, contractName), required);
            return this;
        }

        public FluentLocalComponentConfig AddConstructorValue(object value)
        {
            _concreteTypeRegistration.AddConfiguredConstructorArg(new SimpleValueQuery(value), false);
            return this;
        }

        public FluentLocalComponentConfig AddConstructorValue<TMember>(Func<IComposer, TMember> valueCalculator, bool? required = null)
        {
            _concreteTypeRegistration.AddConfiguredConstructorArg(new FuncValueQuery(c => valueCalculator(c)), required);
            return this;
        }

        public FluentLocalComponentConfig AddConstructorValueFromVariable(string variableName, bool? required = null)
        {
            _concreteTypeRegistration.AddConfiguredConstructorArg(new VariableQuery(variableName), required);
            return this;
        }

        public FluentLocalComponentConfig SetValue(string memberName, object value)
        {
            _concreteTypeRegistration.AddConfiguredInitializationPoint(new SimpleValueQuery(value), memberName);

            return this;
        }

        public FluentLocalComponentConfig SetValue<TMember>(string memberName, Func<IComposer, TMember> valueCalculator, bool? required = null)
        {
            _concreteTypeRegistration.AddConfiguredInitializationPoint(new FuncValueQuery(c => valueCalculator(c)), memberName, null, required);
            return this;
        }

        public FluentLocalComponentConfig SetValueFromVariable(string memberName, string variableName, bool? required = null)
        {
            _concreteTypeRegistration.AddConfiguredInitializationPoint(new VariableQuery(variableName), memberName, null, required);
            return this;
        }

        public FluentLocalComponentConfig NotifyInitialized(Action<IComposer, object> initAction)
        {
            _concreteTypeRegistration.AddCompositionNotification(initAction);
            return this;
        }

        public FluentLocalComponentConfig SetConstructorResolutionPolicy(ConstructorResolutionPolicy policy)
        {
            _concreteTypeRegistration.SetConstructorResolutionPolicy(policy);
            return this;
        }
        
        #endregion
    }
}