using ComposerCore.Attributes;

namespace ComposerCore.Tests.ComponentFactories.Components
{
    [Component, Transient]
    public class SampleComponentTwo : ISampleContractTwo
    {
        [Plug] public ISampleContractOne One { get; set; }
    }
}