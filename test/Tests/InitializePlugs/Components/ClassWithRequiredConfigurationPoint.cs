using ComposerCore.Attributes;

namespace ComposerCore.Tests.InitializePlugs.Components
{
    public class ClassWithRequiredConfigurationPoint
    {
        [ConfigurationPoint("InitPointVariable")]
        public int InitPoint { get; set; }
    }
}