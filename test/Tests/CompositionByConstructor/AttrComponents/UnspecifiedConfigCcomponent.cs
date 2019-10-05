using ComposerCore.Attributes;

namespace ComposerCore.Tests.CompositionByConstructor.AttrComponents
{
    [Contract, Component, Transient]
    public class UnspecifiedConfigCcomponent
    {
        public int Number { get; }
        public string Name { get; }

        public UnspecifiedConfigCcomponent(
            [ConfigurationPoint] int number,
            [ConfigurationPoint] string name)
        {
            Number = number;
            Name = name;
        }
    }
}