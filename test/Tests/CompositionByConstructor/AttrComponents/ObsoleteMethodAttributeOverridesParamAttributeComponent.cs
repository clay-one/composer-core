using ComposerCore.Attributes;

namespace ComposerCore.Tests.CompositionByConstructor.AttrComponents
{
    [Contract, Component, Transient]
    public class ObsoleteMethodAttributeOverridesParamAttributeComponent
    {
        public ISampleContractA A { get; }
        public ISampleContractB B { get; }

        [CompositionConstructor("someName", "anotherName")]
        public ObsoleteMethodAttributeOverridesParamAttributeComponent(
            [ComponentPlug("ignoredName")] ISampleContractA a,
            [ComponentPlug("ignoredName")] ISampleContractB b)
        {
            A = a;
            B = b;
        }
    }
}