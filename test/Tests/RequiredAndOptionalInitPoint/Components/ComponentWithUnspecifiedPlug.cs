using ComposerCore.Attributes;

namespace ComposerCore.Tests.RequiredAndOptionalInitPoint.Components
{
    [Contract, Component, Transient]
    public class ComponentWithUnspecifiedPlug
    {
        [ComponentPlug]
        public IPluggedContract PluggedContract { get; set; }
    }
}