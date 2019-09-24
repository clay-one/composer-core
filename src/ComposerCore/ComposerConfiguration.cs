using ComposerCore.Attributes;

namespace ComposerCore
{
    public class ComposerConfiguration
    {
        public bool DisableAttributeChecking { get; set; }
        
        public bool ConstructorArgumentRequiredByDefault { get; set; }
        public bool ComponentPlugRequiredByDefault { get; set; }
        public bool ConfigurationPointRequiredByDefault { get; set; }
        
        public ConstructorResolutionPolicy DefaultConstructorResolutionPolicy { get; set; }
    }
}