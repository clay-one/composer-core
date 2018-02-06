using ComposerCore.Attributes;
using ComposerCore.Cache;

namespace ComposerCore.Tests.CompositionByConstructor.Components
{
	[Component]
	[ComponentCache(typeof(ContractAgnosticComponentCache))]
	public class SampleComponentB : ISampleContractB
	{
	}
}
