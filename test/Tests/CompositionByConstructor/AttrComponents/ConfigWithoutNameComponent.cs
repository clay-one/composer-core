using ComposerCore.Attributes;

namespace ComposerCore.Tests.CompositionByConstructor.AttrComponents
{
    [Contract, Component, Transient]
    public class ConfigWithoutNameComponent
    {
        public int Number { get; }
        public string Name { get; }

        public ConfigWithoutNameComponent(
            [ConfigurationPoint] int number, 
            [ConfigurationPoint] string name)
        {
            Number = number;
            Name = name;
        }
    }
}