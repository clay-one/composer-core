using System;
using ComposerCore.Attributes;
using ComposerCore.Extensibility;
using ComposerCore.Factories;

namespace ComposerCore.Implementation
{
    public abstract class ComponentBuilderRegistration : ComponentInitializerRegistration
    {
        protected LocalComponentBuilder Builder { get; }
        
        protected ComponentBuilderRegistration(Type targetType) : base(targetType)
        {
            Builder = new LocalComponentBuilder(targetType);
        }

        public override void SetAsRegistered(IComponentContext registrationContext)
        {
            base.SetAsRegistered(registrationContext);
            Builder.Initialize(registrationContext);
        }

        public void SetConstructorResolutionPolicy(ConstructorResolutionPolicy policy)
        {
            Builder.ConstructorResolutionPolicy = policy;
        }

        public void AddConfiguredConstructorArg(ICompositionalQuery query, bool? required = null)
        {
            Builder.AddConfiguredConstructorArg(new ConstructorArgSpecification(required, query));
        }
    }
}