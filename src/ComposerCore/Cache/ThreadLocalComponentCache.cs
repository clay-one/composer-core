using System.Collections.Generic;
using System.Threading;
using ComposerCore.Attributes;
using ComposerCore.Extensibility;

namespace ComposerCore.Cache
{
    [Contract]
    [Component]
    [ComponentCache(typeof(StaticComponentCache))]
    public class ThreadLocalComponentCache : IComponentCache
    {
        private readonly ThreadLocal<IDictionary<ContractIdentity, ComponentCacheEntry>> _cacheContent =
            new ThreadLocal<IDictionary<ContractIdentity, ComponentCacheEntry>>(() =>
                new Dictionary<ContractIdentity, ComponentCacheEntry>());

        private static readonly object StaticSynchronizationObject = new object();

        #region Implementation of IComponentCache

        public ComponentCacheEntry GetFromCache(ContractIdentity contract)
        {
            return _cacheContent.Value.TryGetValue(contract, out var entry) ? entry : null;
        }

        public void PutInCache(ContractIdentity contract, ComponentCacheEntry entry)
        {
            _cacheContent.Value[contract] = entry;
        }

        public object SynchronizationObject => StaticSynchronizationObject;

        #endregion
    }
}