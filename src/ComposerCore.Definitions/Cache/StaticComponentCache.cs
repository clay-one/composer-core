using System.Collections.Generic;

namespace ComposerCore.Definitions.Cache
{
	[Contract]
	[Component]
	[ComponentCache(null)]
	public class StaticComponentCache : IComponentCache
	{
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