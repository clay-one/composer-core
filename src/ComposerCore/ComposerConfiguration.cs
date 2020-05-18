using System.Linq.Expressions;
using ComposerCore.Attributes;

namespace ComposerCore
{
    public class ComposerConfiguration
    {
        public ComposerConfiguration()
        {
            // The default configuration values are targeted to maintain backward compatibility,
            // so that without any changes in configuration Composer behaves exactly as it would
            // in the older versions without the configuration values.
            
            DisableAttributeChecking = false;

            ConstructorArgumentRequiredByDefault = true;
            InitializationPointsRequiredByDefault = true;

            DefaultConstructorResolutionPolicy = ConstructorResolutionPolicy.SingleOrDefault;
        }

        public bool DisableAttributeChecking { get; set; }
        
        public bool ConstructorArgumentRequiredByDefault { get; set; }
        public bool InitializationPointsRequiredByDefault { get; set; }
        
        public ConstructorResolutionPolicy DefaultConstructorResolutionPolicy { get; set; }
    }
}