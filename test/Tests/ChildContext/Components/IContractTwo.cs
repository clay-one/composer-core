using ComposerCore.Attributes;

namespace ComposerCore.Tests.ChildContext.Components
{
    [Contract]
    public interface IContractTwo
    {
    }

    [Component, Transient]
    public class ComponentTwoA : IContractTwo
    {
    }

    [Component, Transient]
    public class ComponentTwoB : IContractTwo
    {
    }
}