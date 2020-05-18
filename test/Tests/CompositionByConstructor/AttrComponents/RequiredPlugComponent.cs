using ComposerCore.Attributes;

namespace ComposerCore.Tests.CompositionByConstructor.AttrComponents
{
    [Contract, Component, Transient]
    public class RequiredPlugComponent
    {
        public ISampleContractA A { get; }
        public ISampleContractB B { get; }

        public RequiredPlugComponent(
            [ComponentPlug(true)] ISampleContractA a,
            [ComponentPlug(true)] ISampleContractB b)
        {
            A = a;
            B = b;
        }
    }
}