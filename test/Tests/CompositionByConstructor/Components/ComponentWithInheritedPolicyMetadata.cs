using ComposerCore.Attributes;

namespace ComposerCore.Tests.CompositionByConstructor.Components
{
    [Component, Contract, Transient]
    public class ComponentWithInheritedPolicyMetadata : ComponentWithDirectPolicyMetadata
    {
        public ComponentWithInheritedPolicyMetadata(ISampleContractA a) : base(a)
        {
        }

        public ComponentWithInheritedPolicyMetadata(ISampleContractA a, ISampleContractB b) : base(a, b)
        {
        }
    }
}