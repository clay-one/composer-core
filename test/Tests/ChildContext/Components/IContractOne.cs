using ComposerCore.Attributes;

namespace ComposerCore.Tests.ChildContext.Components
{
    [Contract]
    public interface IContractOne
    {
    }

    [Component, Transient]
    public class ComponentOneA : IContractOne
    {
    }

    [Component, Transient]
    public class ComponentOneB : IContractOne
    {
    }

    [Component, Singleton]
    public class ComponentOneSingleton : IContractOne
    {
    }
}