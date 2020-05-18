using ComposerCore.Attributes;

namespace ComposerCore.Tests.RequiredAndOptionalInitPoint.Components
{
    [Contract, Component, Transient]
    public class ComponentWithUnspecifiedNamedPlug
    {
        [ComponentPlug("contractName")]
        public IPluggedContract PluggedContract { get; set; }
    }
}