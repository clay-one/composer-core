using ComposerCore.Attributes;

namespace ComposerCore.Tests.CompositionByConstructor.AttrComponents
{
    [Contract, Component, Transient]
    public class UnspecifiedPlugComponent
    {
        public ISampleContractA A { get; }
        public ISampleContractB B { get; }

        public UnspecifiedPlugComponent(
            [ComponentPlug] ISampleContractA a,
            ISampleContractB b)
        {
            A = a;
            B = b;
        }
    }
}