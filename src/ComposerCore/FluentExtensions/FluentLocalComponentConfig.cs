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

        #region Constructors

        public FluentLocalComponentConfig(ComponentContext context, LocalComponentFactory factory)
            : base(context)
        {
            Factory = factory ?? throw new ArgumentNullException(nameof(factory));
            Registration = new ComponentRegistration(Factory);
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
            Factory.InitializationPoints.Add(new InitializationPointSpecification(memberName, MemberTypes.All,
                required, new ComponentQuery(contractType, contractName)));

            return this;
        }

        public FluentLocalComponentConfig UseConstructor(params Type[] argTypes)
        {
            var constructor = Factory.TargetType.GetConstructor(argTypes);
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
            Factory.ConstructorResolutionPolicy = ConstructorResolutionPolicy.Preset;
            
            return this;
        }

        public FluentLocalComponentConfig AddConstructorComponent<TPlugContract>(string contractName = null, bool? required = null)
        {
            return AddConstructorComponent(typeof(TPlugContract), contractName, required);
        }

        public FluentLocalComponentConfig AddConstructorComponent(Type contractType, string contractName = null, bool? required = null)
        {
            Factory.AddConfiguredConstructorArg(new ConstructorArgSpecification(required, new ComponentQuery(contractType, contractName)));
            return this;
        }

        public FluentLocalComponentConfig AddConstructorValue(object value)
        {
            Factory.AddConfiguredConstructorArg(new ConstructorArgSpecification(false, new SimpleValueQuery(value)));
            return this;
        }

        public FluentLocalComponentConfig AddConstructorValue<TMember>(Func<IComposer, TMember> valueCalculator, bool? required = null)
        {
            Factory.AddConfiguredConstructorArg(new ConstructorArgSpecification(required, 
                new FuncValueQuery(c => valueCalculator(c))));

            return this;
        }

        public FluentLocalComponentConfig AddConstructorValueFromVariable(string variableName, bool? required = null)
        {
            Factory.AddConfiguredConstructorArg(new ConstructorArgSpecification(required, new VariableQuery(variableName)));
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

        public FluentLocalComponentConfig SetConstructorResolutionPolicy(ConstructorResolutionPolicy policy)
        {
            Factory.ConstructorResolutionPolicy = policy;
            return this;
        }
        
        #endregion
    }
}