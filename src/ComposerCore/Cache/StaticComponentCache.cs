using System.Collections.Generic;
using ComposerCore.Attributes;
using ComposerCore.Extensibility;

namespace ComposerCore.Cache
{
	[Contract, Component, ComponentCache(null), ConstructorResolutionPolicy(null)]
	public class StaticComponentCache : IComponentCache
	{
		[CompositionConstructor]
		public StaticComponentCache()
		{
		}
		
		private static readonly IDictionary<ContractIdentity, ComponentCacheEntry> CacheContent =
			new Dictionary<ContractIdentity, ComponentCacheEntry>();

		private static readonly object StaticSynchronizationObject = new object();

		#region Implementation of IComponentCache

		public ComponentCacheEntry GetFromCache(ContractIdentity contract)
		{
		    return CacheContent.TryGetValue(contract, out var entry) ? entry : null;
		}

		public void PutInCache(ContractIdentity contract, ComponentCacheEntry entry)
		{
			CacheContent[contract] = entry;
		}

		public object SynchronizationObject => StaticSynchronizationObject;

	    #endregion
	}
}