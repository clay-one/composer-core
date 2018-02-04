using ComposerCore.Definitions;
using ComposerCore.Definitions.Cache;

namespace ComposerCore.Tests.CompositionListener.Components
{
	[Component]
	[ComponentCache(typeof(ContractAgnosticComponentCache))]
	public class SharedComponent : ISampleContract
	{
	}
}
