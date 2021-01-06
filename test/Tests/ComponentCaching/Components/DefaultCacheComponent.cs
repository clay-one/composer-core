using ComposerCore.Attributes;
using ComposerCore.Cache;

namespace ComposerCore.Tests.ComponentCaching.Components
{
	[Contract]
	[Component]
	[ComponentCache(typeof(DefaultComponentCache))]
	public class DefaultCacheComponent : ISomeContract, IAnotherContract
	{
		[ComponentPlug]
		public IComposer Composer { get; set; }
	}

	[Contract]
	[Component]
	[ComponentCache(typeof(DefaultComponentCache))]
	public class DefaultCacheComponentWithPlugs
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
