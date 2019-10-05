using ComposerCore.Attributes;

namespace ComposerCore.Tests.CompositionByConstructor.AttrComponents
{
    [Contract, Component, Transient]
    public class PlugWithContractNameComponent
    {
        public ISampleContractA A { get; }
        public ISampleContractB B { get; }

        public PlugWithContractNameComponent(
            [ComponentPlug("someName")] ISampleContractA a,
            [ComponentPlug("anotherName")] ISampleContractB b)
        {
            A = a;
            B = b;
        }
    }
}