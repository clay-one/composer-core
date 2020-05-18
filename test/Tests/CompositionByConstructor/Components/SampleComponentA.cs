using ComposerCore.Attributes;

namespace ComposerCore.Tests.CompositionByConstructor.Components
{
	[Component, Transient]
	public class SampleComponentA : ISampleContractA
	{
	}
}
