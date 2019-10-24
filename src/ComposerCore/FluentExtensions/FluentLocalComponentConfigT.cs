using System;
using System.Linq.Expressions;
using System.Reflection;
using ComposerCore.Attributes;
using ComposerCore.Cache;
using ComposerCore.CompositionalQueries;
using ComposerCore.Factories;
using ComposerCore.Implementation;

namespace ComposerCore.FluentExtensions
{
    public class FluentLocalComponentConfig<TComponent> : FluentLocalComponentConfig
    {
        #region Constructors

        public FluentLocalComponentConfig(ComponentContext context) 
            : base(context, new LocalComponentFactory(typeof(TComponent)))
        {
        }

        #endregion

        #region Fluent configuration methods

        public new FluentLocalComponentConfig<TComponent> SetComponent<TPlugContract>(
            string memberName, string contractName = null, bool? required = null)
        {
            return SetComponent(memberName, typeof(TPlugContract), contractName, required);
        }

        public new FluentLocalComponentConfig<TComponent> SetComponent(
            string memberName, Type contractType, string contractName = null, bool? required = null)
        {
            base.SetComponent(memberName, contractType, contractName, required);
            return this;
        }

        public FluentLocalComponentConfig<TComponent> SetComponent<TPlugContract>(
            Expression<Func<TComponent, TPlugContract>> member, string contractName = null, bool? required = null)
        {
            if (!(member.Body is MemberExpression memberExpression) ||
                !(memberExpression.Expression is ParameterExpression parameterExpression) ||
                parameterExpression.Type != typeof(TComponent))
            {
                throw new ArgumentException("Member pointer should point to an immediate member. " +
                                            "The only acceptable expression format is <x => x.MemberName>.");
            }
            
            _concreteComponentRegistration.AddConfiguredInitializationPoint(
                new ComponentQuery(typeof(TPlugContract), contractName), 
                memberExpression.Member.Name, memberExpression.Member.MemberType, required);
            
            return this;
        }

        public new FluentLocalComponentConfig<TComponent> UseConstructor(params Type[] argTypes)
        {
            base.UseConstructor(argTypes);
            return this;
        }

        public new FluentLocalComponentConfig<TComponent> UseConstructor(ConstructorInfo constructorInfo)
        {
            base.UseConstructor(constructorInfo);
            return this;
        }

        public new FluentLocalComponentConfig<TComponent> AddConstructorComponent<TPlugContract>(string contractName = null, bool? required = null)
        {
            base.AddConstructorComponent<TPlugContract>(contractName, required);
            return this;
        }

        public new FluentLocalComponentConfig<TComponent> AddConstructorComponent(Type contractType, string contractName = null, bool? required = null)
        {
            base.AddConstructorComponent(contractType, contractName, required);
            return this;
        }

        public new FluentLocalComponentConfig<TComponent> AddConstructorValue(object value)
        {
            base.AddConstructorValue(value);
            return this;
        }

        public new FluentLocalComponentConfig<TComponent> AddConstructorValue<TValue>(Func<IComposer, TValue> valueCalculator, bool? required = null)
        {
            base.AddConstructorValue(valueCalculator, required);
            return this;
        }

        public new FluentLocalComponentConfig<TComponent> AddConstructorValueFromVariable(string variableName, bool? required = null)
        {
            base.AddConstructorValueFromVariable(variableName, required);
            return this;
        }

        public new FluentLocalComponentConfig<TComponent> SetValue(string memberName, object value)
        {
            base.SetValue(memberName, value);
            return this;
        }

        public FluentLocalComponentConfig<TComponent> SetValue<TMember>(Expression<Func<TComponent, TMember>> member, TMember value)
        {
            if (!(member.Body is MemberExpression memberExpression) ||
                !(memberExpression.Expression is ParameterExpression parameterExpression) ||
                parameterExpression.Type != typeof(TComponent))
            {
                throw new ArgumentException("Member pointer should point to an immediate member. " +
                                            "The only acceptable expression format is <x => x.MemberName>.");
            }

            _concreteComponentRegistration.AddConfiguredInitializationPoint(new SimpleValueQuery(value), 
                memberExpression.Member.Name, memberExpression.Member.MemberType, false);

            return this;
        }

        public FluentLocalComponentConfig<TComponent> SetValue<TMember>(Expression<Func<TComponent, TMember>> member,
            Func<IComposer, TMember> valueCalculator, bool? required = false)
        {
            if (!(member.Body is MemberExpression memberExpression) ||
                !(memberExpression.Expression is ParameterExpression parameterExpression) ||
                parameterExpression.Type != typeof(TComponent))
            {
                throw new ArgumentException("Member pointer should point to an immediate member. " +
                                            "The only acceptable expression format is <x => x.MemberName>.");
            }

            _concreteComponentRegistration.AddConfiguredInitializationPoint(new FuncValueQuery(c => valueCalculator(c)), 
                memberExpression.Member.Name, memberExpression.Member.MemberType, required);

            return this;
        }

        public new FluentLocalComponentConfig<TComponent> SetValue<TMember>(string memberName, Func<IComposer, TMember> valueCalculator, bool? required = null)
        {
            base.SetValue(memberName, valueCalculator, required);
            return this;
        }

        public new FluentLocalComponentConfig<TComponent> SetValueFromVariable(string memberName, string variableName, bool? required = null)
        {
            base.SetValueFromVariable(memberName, variableName, required);
            return this;
        }

        public FluentLocalComponentConfig<TComponent> SetValueFromVariable<TMember>(
            Expression<Func<TComponent, TMember>> member, string variableName, bool? required = null)
        {
            if (!(member.Body is MemberExpression memberExpression) ||
                !(memberExpression.Expression is ParameterExpression parameterExpression) ||
                parameterExpression.Type != typeof(TComponent))
            {
                throw new ArgumentException("Member pointer should point to an immediate member. " +
                                            "The only acceptable expression format is <x => x.MemberName>.");
            }

            _concreteComponentRegistration.AddConfiguredInitializationPoint(new VariableQuery(variableName), 
                memberExpression.Member.Name, memberExpression.Member.MemberType, required);

            return this;
        }

        public FluentLocalComponentConfig<TComponent> NotifyInitialized(Action<IComposer, TComponent> initAction)
        {
            base.NotifyInitialized((c, o) => initAction(c, (TComponent)o));
            return this;
        }

        public new FluentLocalComponentConfig<TComponent> SetConstructorResolutionPolicy(ConstructorResolutionPolicy policy)
        {
            base.SetConstructorResolutionPolicy(policy);
            return this;
        }

        public new FluentLocalComponentConfig<TComponent> UseComponentCache(Type cacheContractType)
        {
            Registration.SetCache(cacheContractType == null ? nameof(NoComponentCache) : cacheContractType.Name);
            return this;
        }

        public new FluentLocalComponentConfig<TComponent> UseComponentCache<TCacheContract>()
        {
            return UseComponentCache(typeof(TCacheContract));
        }

        public new FluentLocalComponentConfig<TComponent> AsSingleton()
        {
            return UseComponentCache(typeof(ContractAgnosticComponentCache));
        }

        public new FluentLocalComponentConfig<TComponent> AsTransient()
        {
            return UseComponentCache(null);
        }

        #endregion
    }
}