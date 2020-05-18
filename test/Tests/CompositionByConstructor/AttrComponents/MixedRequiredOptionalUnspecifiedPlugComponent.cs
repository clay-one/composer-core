using ComposerCore.Attributes;

namespace ComposerCore.Tests.CompositionByConstructor.AttrComponents
{
    [Contract, Component, Transient]
    public class MixedRequiredOptionalUnspecifiedPlugComponent
    {
                public ISampleContractA A { get; }
                public ISampleContractB B { get; }
                public ISampleContractC C { get; }
                public ISampleContractD D { get; }

                public MixedRequiredOptionalUnspecifiedPlugComponent(
                    ISampleContractA a, 
                    [ComponentPlug] ISampleContractB b, 
                    [ComponentPlug(false)] ISampleContractC c, 
                    [ComponentPlug(true)] ISampleContractD d)
                {
                    A = a;
                    B = b;
                    C = c;
                    D = d;
                }
    }
}