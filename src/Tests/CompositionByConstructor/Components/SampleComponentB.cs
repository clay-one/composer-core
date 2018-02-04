using ComposerCore.Definitions;
using ComposerCore.Definitions.Cache;

namespace ComposerCore.Tests.CompositionByConstructor.Components
{
	[Component]
	[ComponentCache(typeof(ContractAgnosticComponentCache))]
	public class SampleComponentB : ISampleContractB
	{
	}
}
