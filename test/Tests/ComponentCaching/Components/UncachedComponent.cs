using ComposerCore.Attributes;

namespace ComposerCore.Tests.ComponentCaching.Components
{
	[Contract]
	[Component]
	[ComponentCache(null)]
	public class UncachedComponent : ISomeContract, IAnotherContract
	{
	}

	[Contract]
	[Component]
	[Transient]
	public class TransientComponent : ISomeContract
	{
	}

	[Contract]
	[Component]
	[ComponentCache(null)]
	public class UncachedComponentWithPlugs
	{
		[ComponentPlug]
		public ContractAgnosticComponent ContractAgnosticComponent { get; set; }

		[ComponentPlug]
		public DefaultCacheComponent DefaultCacheComponent { get; set; }

		[ComponentPlug]
		public UncachedComponent UncachedComponent { get; set; }
	}
}
