using ComposerCore.Attributes;

namespace ComposerCore.Tests.FluentRegistration.Components
{
    [Component]
    public class ComponentWithManyContracts : IComponentOne, IComponentTwo
    {
        
    }
}