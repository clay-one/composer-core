using ComposerCore.Attributes;

namespace ComposerCore.Tests.CompositionByConstructor.AttrComponents
{
    [Contract, Component, Transient]
    public class OptionalPlugComponent
    {
        public ISampleContractA A { get; }
        public ISampleContractB B { get; }

        public OptionalPlugComponent(
            [ComponentPlug(false)] ISampleContractA a,
            [ComponentPlug(false)] ISampleContractB b)
        {
            A = a;
            B = b;
        }

    }
}