using ComposerCore.Attributes;

namespace ComposerCore.Tests.ComponentCaching.Components
{
	[Contract]
	[Component]
	[ComponentCache(null)]
	public class UncachedComponent : ISomeContract, IAnotherContract
	{
		[ComponentPlug]
		public IComposer Composer { get; set; }
	}

	[Contract]
	[Component]
	[Transient]
	public class TransientComponent : ISomeContract
	{
		[ComponentPlug]
		public IComposer Composer { get; set; }
	}

	[Contract]
	[Component]
	[ComponentCache(null)]
	public class UncachedComponentWithPlugs
	{
		[ComponentPlug]
		public IComposer Composer { get; set; }

		[ComponentPlug]
		public ScopedComponent ScopedComponent { get; set; }

		[ComponentPlug]
		public ContractAgnosticComponent ContractAgnosticComponent { get; set; }

		[ComponentPlug]
		public DefaultCacheComponent DefaultCacheComponent { get; set; }

		[ComponentPlug]
		public UncachedComponent UncachedComponent { get; set; }
	}
}
