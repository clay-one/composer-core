using ComposerCore.Attributes;

namespace ComposerCore.Tests.CompositionByConstructor.AttrComponents
{
    [Contract, Component, Transient]
    public class ConfigWithNameComponent
    {
        public int Number { get; }
        public string Name { get; }

        public ConfigWithNameComponent(
            [ConfigurationPoint("theNumber")] int number, 
            [ConfigurationPoint("theName")] string name)
        {
            Number = number;
            Name = name;
        }
    }
}