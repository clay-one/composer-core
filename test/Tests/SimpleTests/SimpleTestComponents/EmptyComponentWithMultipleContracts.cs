using ComposerCore.Attributes;

namespace ComposerCore.Tests.SimpleTests.SimpleTestComponents
{
    [Contract, Component]
    public class EmptyComponentWithMultipleContracts : EmptyComponentAndContract, IEmptyContract
    {
    }
}