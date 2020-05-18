using ComposerCore.Attributes;

namespace ComposerCore.Tests.InitializePlugs.Components
{
    public class ClassWithRequiredComponentPlug
    {
        [ComponentPlug]
        public ISampleContract SampleContract { get; set; }
    }
}