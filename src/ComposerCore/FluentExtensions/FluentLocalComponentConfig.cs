using System;
using System.Reflection;
using ComposerCore.Attributes;
using ComposerCore.CompositionalQueries;
using ComposerCore.Extensibility;
using ComposerCore.Factories;
using ComposerCore.Implementation;

namespace ComposerCore.FluentExtensions
{
    public class FluentLocalComponentConfig : FluentComponentConfigBase<FluentLocalComponentConfig>
    {
        protected readonly LocalComponentFactory Factory;
        protected readonly ConcreteComponentRegistration _concreteComponentRegistration;

        protected override IComponentRegistration Registration => _concreteComponentRegistration;

        #region Constructors

        public FluentLocalComponentConfig(ComponentContext context, LocalComponentFactory factory)
            : base(context)
        {
            Factory = factory ?? throw new ArgumentNullException(nameof(factory));
            _concreteComponentRegistration = new ConcreteComponentRegistration(Factory);
        }

        #endregion
        
        #region Terminating methods
        
        public void RegisterAsItself()
        {
            RegisterWith(Factory.TargetType);
        }

        public void RegisterAsItself(string contractName)
        {
            RegisterWith(Factory.TargetType, contractName);
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
            _concreteComponentRegistration.AddConfiguredInitializationPoint(
                new ComponentQuery(contractType, contractName), memberName, null, required);

            return this;
        }

        public FluentLocalComponentConfig UseConstructor(params Type[] argTypes)
        {
            var constructor = _concreteComponentRegistration.TargetType.GetConstructor(argTypes);
            if (constructor == null)
                throw new ArgumentException("Could not find a public constructor with the specified parameter types " +
                                            $"in the class '{Factory.TargetType.FullName}'");

            return UseConstructor(constructor);
        }

        public FluentLocalComponentConfig UseConstructor(ConstructorInfo constructorInfo)
        {
            if (constructorInfo == null)
                throw new ArgumentNullException(nameof(constructorInfo));
            
            Context.GetComponent<IPresetConstructorStore>().SetConstructor(Factory.TargetType, constructorInfo);
            _concreteComponentRegistration.SetConstructorResolutionPolicy(ConstructorResolutionPolicy.Preset);
            
            return this;
        }

        public FluentLocalComponentConfig AddConstructorComponent<TPlugContract>(string contractName = null, bool? required = null)
        {
            return AddConstructorComponent(typeof(TPlugContract), contractName, required);
        }

        public FluentLocalComponentConfig AddConstructorComponent(Type contractType, string contractName = null, bool? required = null)
        {
            _concreteComponentRegistration.AddConfiguredConstructorArg(new ComponentQuery(contractType, contractName), required);
            return this;
        }

        public FluentLocalComponentConfig AddConstructorValue(object value)
        {
            _concreteComponentRegistration.AddConfiguredConstructorArg(new SimpleValueQuery(value), false);
            return this;
        }

        public FluentLocalComponentConfig AddConstructorValue<TMember>(Func<IComposer, TMember> valueCalculator, bool? required = null)
        {
            _concreteComponentRegistration.AddConfiguredConstructorArg(new FuncValueQuery(c => valueCalculator(c)), required);
            return this;
        }

        public FluentLocalComponentConfig AddConstructorValueFromVariable(string variableName, bool? required = null)
        {
            _concreteComponentRegistration.AddConfiguredConstructorArg(new VariableQuery(variableName), required);
            return this;
        }

        public FluentLocalComponentConfig SetValue(string memberName, object value)
        {
            _concreteComponentRegistration.AddConfiguredInitializationPoint(new SimpleValueQuery(value), memberName);

            return this;
        }

        public FluentLocalComponentConfig SetValue<TMember>(string memberName, Func<IComposer, TMember> valueCalculator, bool? required = null)
        {
            _concreteComponentRegistration.AddConfiguredInitializationPoint(new FuncValueQuery(c => valueCalculator(c)), memberName, null, required);
            return this;
        }

        public FluentLocalComponentConfig SetValueFromVariable(string memberName, string variableName, bool? required = null)
        {
            _concreteComponentRegistration.AddConfiguredInitializationPoint(new VariableQuery(variableName), memberName, null, required);
            return this;
        }

        public FluentLocalComponentConfig NotifyInitialized(Action<IComposer, object> initAction)
        {
            _concreteComponentRegistration.AddCompositionNotification(initAction);
            return this;
        }

        public FluentLocalComponentConfig SetConstructorResolutionPolicy(ConstructorResolutionPolicy policy)
        {
            _concreteComponentRegistration.SetConstructorResolutionPolicy(policy);
            return this;
        }
        
        #endregion
    }
}