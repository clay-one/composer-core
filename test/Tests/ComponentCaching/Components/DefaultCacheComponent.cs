using ComposerCore.Attributes;
using ComposerCore.Cache;

namespace ComposerCore.Tests.ComponentCaching.Components
{
	[Contract]
	[Component]
	[ComponentCache(typeof(DefaultComponentCache))]
	public class DefaultCacheComponent : ISomeContract, IAnotherContract
	{
	}

	[Contract]
	[Component]
	[ComponentCache(typeof(DefaultComponentCache))]
	public class DefaultCacheComponentWithPlugs
	{
		[ComponentPlug]
		public ContractAgnosticComponent ContractAgnosticComponent { get; set; }

		[ComponentPlug]
		public DefaultCacheComponent DefaultCacheComponent { get; set; }

		[ComponentPlug]
		public UncachedComponent UncachedComponent { get; set; }
	}
}
