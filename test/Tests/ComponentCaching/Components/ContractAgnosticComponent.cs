using ComposerCore.Attributes;
using ComposerCore.Cache;

namespace ComposerCore.Tests.ComponentCaching.Components
{
	[Contract, Component, ComponentCache(typeof(ContractAgnosticComponentCache))]
	public class ContractAgnosticComponent : ISomeContract, IAnotherContract
	{
	}

	[Contract, Component, Singleton]
	public class SingletonComponent : IAnotherContract
	{
	}
	
	[Contract, Component, ComponentCache(typeof(ContractAgnosticComponentCache))]
	public class ContractAgnosticComponentWithPlugs
	{
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
