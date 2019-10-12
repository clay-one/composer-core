using System;
using System.Collections.Generic;
using ComposerCore.Attributes;
using Microsoft.AspNetCore.Http;

namespace ComposerCore.AspNet
{
    [Contract]
    [Component]
    [ComponentCache(null)]
    public class AspNetCoreRequestComponentCache
    {
        private static readonly string ContextKey = typeof(AspNetCoreRequestComponentCache).FullName;
        
        private readonly IHttpContextAccessor _contextAccessor;

        public AspNetCoreRequestComponentCache(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public object GetFromCache(ContractIdentity contract)
        {
            var context = _contextAccessor.HttpContext;

            var cacheStore = context?.Items[ContextKey] as Dictionary<ContractIdentity, object>;
            if (cacheStore == null)
                return null;

            return cacheStore.TryGetValue(contract, out var entry) ? entry : null;
        }

        public void PutInCache(ContractIdentity contract, object entry)
        {
            var context = _contextAccessor.HttpContext;
            if (context == null)
                throw new InvalidOperationException("A scoped component should not be requested / initialized outside scope");

            Dictionary<ContractIdentity, object> cacheStore = null;
            if (context.Items.ContainsKey(ContextKey))
                cacheStore = context.Items[ContextKey] as Dictionary<ContractIdentity, object>;
            
            if (cacheStore == null)
            {
                cacheStore = new Dictionary<ContractIdentity, object>();
                context.Items[ContextKey] = cacheStore;
            }

            cacheStore[contract] = entry;
        }

        public object SynchronizationObject => _contextAccessor.HttpContext?.TraceIdentifier;
    }
}