using System.Collections.Generic;
using ComposerCore.Attributes;
using ComposerCore.Extensibility;

namespace ComposerCore.Cache
{
	[Contract, Component, ComponentCache(null), ConstructorResolutionPolicy(null)]
	public class DefaultComponentCache : IComponentCache
	{
		[CompositionConstructor]
		public DefaultComponentCache()
		{
		}
		
		private readonly IDictionary<ContractIdentity, ComponentCacheEntry> _cacheContent =
			new Dictionary<ContractIdentity, ComponentCacheEntry>();

		#region Implementation of IComponentCache

		public ComponentCacheEntry GetFromCache(ContractIdentity contract)
		{
		    return _cacheContent.TryGetValue(contract, out var entry) ? entry : null;
		}

		public void PutInCache(ContractIdentity contract, ComponentCacheEntry entry)
		{
			_cacheContent[contract] = entry;
		}

		public object SynchronizationObject => this;

	    #endregion
	}
}