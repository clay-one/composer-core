using System;

namespace ComposerCore.Implementation
{
    public class FactoryMethodRegistration : ComponentInitializerRegistration
    {
        public FactoryMethodRegistration(Type targetType) : base(targetType)
        {
        }

        public override void AddContract(ContractIdentity contract)
        {
            throw new NotImplementedException();
        }

        public override bool IsResolvable(Type contractType)
        {
            throw new NotImplementedException();
        }

        public override object GetComponent(ContractIdentity contract, IComposer dependencyResolver)
        {
            throw new NotImplementedException();
        }

        protected override void ReadContractsFromTarget()
        {
            throw new NotImplementedException();
        }

        protected override void EnsureComponentAttribute()
        {
            throw new NotImplementedException();
        }
    }
}