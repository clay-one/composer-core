using System;
using ComposerCore.Extensibility;

namespace ComposerCore.Implementation
{
    public class GenericComponentRegistration : ComponentRegistration
    {
        public GenericComponentRegistration(IComponentFactory factory) : base(factory)
        {
        }

        public GenericComponentRegistration(IComponentFactory factory, string componentCacheName) : base(factory, componentCacheName)
        {
        }

        public GenericComponentRegistration(IComponentFactory factory, IComponentCache cache) : base(factory, cache)
        {
        }
        
        public override bool IsResolvable(Type contractType)
        {
            var requestedClosedContractType = contractType;
            if (!requestedClosedContractType.IsGenericType || requestedClosedContractType.ContainsGenericParameters)
                return false;

            return MapToClosedComponentType(contractType) != null;
        }

        private Type MapToClosedComponentType(Type contractType)
        {
            throw new NotImplementedException();
        }
    }
}