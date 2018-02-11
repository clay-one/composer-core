using ComposerCore.Attributes;
using ComposerCore.Cache;

namespace ComposerCore.Tests.CompositionListener.Components
{
	[Component]
	[ComponentCache(typeof(ContractAgnosticComponentCache))]
	public class SharedComponent : ISampleContract
	{
	}
}
