using System;

namespace ComposerCore.Implementation
{
    public class PreInitializedComponentRegistration : ComponentRegistrationBase
    {
        private readonly object _componentInstance;

        public PreInitializedComponentRegistration(object componentInstance) 
            : base((componentInstance ?? throw new ArgumentNullException(nameof(componentInstance))).GetType())
        {
            _componentInstance = componentInstance;
        }
        
        public override object GetComponent(ContractIdentity contract, IComposer scope)
        {
            return _componentInstance;
        }

        public override object CreateComponent(ContractIdentity contract, IComposer scope)
        {
            throw new InvalidOperationException(
                "CreateComponent should never be called on an PreInitializedComponentRegistration object. " +
                "The component instance can be retrieved using GetComponent method.");
        }
    }
}