using System;

namespace ComposerCore.Implementation
{
    internal class ComposerSelfRegistration : ComponentRegistrationBase
    {
        private readonly IComposer _componentInstance;

        public ComposerSelfRegistration(IComposer componentInstance) 
            : base((componentInstance ?? throw new ArgumentNullException(nameof(componentInstance))).GetType())
        {
            _componentInstance = componentInstance;
        }
        
        public override object GetComponent(ContractIdentity contract, IComposer scope)
        {
            return contract.Type == typeof(IComposer) ? scope : _componentInstance;
        }

        public override object CreateComponent(ContractIdentity contract, IComposer scope)
        {
            throw new InvalidOperationException(
                "CreateComponent should never be called on an ComposerSelfRegistration object. " +
                "The component instance can be retrieved using GetComponent method.");
        }
    }
}