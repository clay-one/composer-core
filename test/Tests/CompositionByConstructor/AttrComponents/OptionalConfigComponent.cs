using ComposerCore.Attributes;

namespace ComposerCore.Tests.CompositionByConstructor.AttrComponents
{
    [Contract, Component, Transient]
    public class OptionalConfigComponent
    {
        public int Number { get; }
        public string Name { get; }

        public OptionalConfigComponent(
            [ConfigurationPoint(false)] int number, 
            [ConfigurationPoint(false)] string name)
        {
            Number = number;
            Name = name;
        }
    }
}