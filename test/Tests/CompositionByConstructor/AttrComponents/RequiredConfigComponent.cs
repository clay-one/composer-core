using ComposerCore.Attributes;

namespace ComposerCore.Tests.CompositionByConstructor.AttrComponents
{
    [Contract, Component, Transient]
    public class RequiredConfigComponent
    {
        public int Number { get; }
        public string Name { get; }

        public RequiredConfigComponent(
            [ConfigurationPoint(true)] int number, 
            [ConfigurationPoint(true)] string name)
        {
            Number = number;
            Name = name;
        }
    }
}