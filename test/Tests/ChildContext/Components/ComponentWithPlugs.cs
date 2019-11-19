using ComposerCore.Attributes;

namespace ComposerCore.Tests.ChildContext.Components
{
    [Contract, Component, Transient]
    public class ComponentWithPlugs
    {
        [Plug] public IContractOne One { get; set; }
        [Plug] public IContractTwo Two { get; set; }
    }
}