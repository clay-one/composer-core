using ComposerCore.Attributes;

namespace ComposerCore.Tests.FluentRegistration.Components
{
    [Component, Transient]
    public class TransientComponent : IComponentOne
    {
        
    }
}