using ComposerCore.Attributes;

namespace ComposerCore.Tests.RequiredAndOptionalInitPoint.Components
{
    [Contract, Component, Transient]
    public class ComponentWithUnspecifiedConfig
    {
        [ConfigurationPoint]
        public string SomeConfig { get; set; }
    }
}