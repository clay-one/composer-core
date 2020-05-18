using ComposerCore.Attributes;
using ComposerCore.Extensibility;

namespace ComposerCore.Cache
{
	[Contract, Component, ComponentCache(null), ConstructorResolutionPolicy(null)]
	public class ContractAgnosticComponentCache : IComponentCache
	{
		private ComponentCacheEntry _cacheContent;

		[CompositionConstructor]
		public ContractAgnosticComponentCache()
		{
		}
		
		#region Implementation of IComponentCache

		public ComponentCacheEntry GetFromCache(ContractIdentity contract)
		{
			return _cacheContent;
		}

		public void PutInCache(ContractIdentity contract, ComponentCacheEntry entry)
		{
			_cacheContent = entry;
		}

		public object SynchronizationObject => this;

	    #endregion
	}
}