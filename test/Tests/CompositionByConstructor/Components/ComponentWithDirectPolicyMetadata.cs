using ComposerCore.Attributes;

namespace ComposerCore.Tests.CompositionByConstructor.Components
{
    [Component, Contract, Transient, ConstructorResolutionPolicy(ConstructorResolutionPolicy.MostResolvable)]
    public class ComponentWithDirectPolicyMetadata
    {
        public ComponentWithDirectPolicyMetadata(ISampleContractA a)
        {
            A = a;
            InvokedConstructor = 2;
        }

        public ComponentWithDirectPolicyMetadata(ISampleContractA a, ISampleContractB b)
        {
            A = a;
            B = b;
            InvokedConstructor = 3;
        }

        public ISampleContractA A { get; }

        public ISampleContractB B { get; }

        public int InvokedConstructor { get; }

    }
}