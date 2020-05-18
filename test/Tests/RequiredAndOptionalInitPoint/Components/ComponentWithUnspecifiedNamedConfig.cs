using ComposerCore.Attributes;

namespace ComposerCore.Tests.RequiredAndOptionalInitPoint.Components
{
    [Contract, Component, Transient]
    public class ComponentWithUnspecifiedNamedConfig
    {
        [ConfigurationPoint("someVariable")]
        public string SomeConfig { get; set; }
    }
}