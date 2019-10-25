using System;
using System.Reflection;
using ComposerCore.Attributes;
using ComposerCore.Extensibility;
using ComposerCore.Factories;

namespace ComposerCore.Implementation
{
    public abstract class ComponentInitializerRegistration : ComponentRegistrationBase
    {
        protected LocalComponentInitializer Initializer { get; }

        protected ComponentInitializerRegistration(Type targetType) : base(targetType)
        {
            Initializer = new LocalComponentInitializer(targetType ?? typeof(object));
        }

        public void CopyConfigFrom(ComponentInitializerRegistration original)
        {
            CopyConfigFrom((ComponentRegistrationBase)original);
            Initializer.CopyConfigFrom(original.Initializer);
        }

        public override void SetAsRegistered(IComponentContext registrationContext)
        {
            base.SetAsRegistered(registrationContext);
            Initializer.Initialize(registrationContext);
        }

        public void AddCompositionNotification(Action<IComposer, object> initAction)
        {
            Initializer.AddCompositionNotification(initAction);
        }

        public void AddConfiguredInitializationPoint(ICompositionalQuery query, string memberName,
            MemberTypes? memberType = null, bool? required = null)
        {
            Initializer.AddConfiguredInitializationPoint(
                new InitializationPointSpecification(memberName, memberType ?? MemberTypes.All, required, query));
        }
        
    }
}