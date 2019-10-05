using ComposerCore.Attributes;

namespace ComposerCore.Tests.CompositionByConstructor.AttrComponents
{
    [Contract]
    public interface ISampleContractA
    {
    }

    [Contract]
    public interface ISampleContractB
    {
    }
    
    [Contract]
    public interface ISampleContractC
    {
    }
    
    [Contract]
    public interface ISampleContractD
    {
    }
    
    [Component, Transient]
    public class SampleComponentA : ISampleContractA
    {
    }

    [Component, Transient]
    public class SampleComponentB : ISampleContractB
    {
    }

    [Component, Transient]
    public class SampleComponentC : ISampleContractC
    {
    }

    [Component, Transient]
    public class SampleComponentD : ISampleContractD
    {
    }
}