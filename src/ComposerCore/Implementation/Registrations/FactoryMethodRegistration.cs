using System;

namespace ComposerCore.Implementation
{
    public class FactoryMethodRegistration : ComponentInitializerRegistration
    {
        public FactoryMethodRegistration(Type targetType) : base(targetType)
        {
        }
        
        public override object GetComponent(ContractIdentity contract, IComposer dependencyResolver)
        {
            throw new NotImplementedException();
        }

        public override object CreateComponent(ContractIdentity contract, IComposer dependencyResolver)
        {
            throw new NotImplementedException();
        }
    }
}