using System.Collections.Concurrent;
using ComposerCore.Attributes;
using ComposerCore.Extensibility;
using ComposerCore.Implementation;

namespace ComposerCore.Cache
{
	[Component(nameof(StaticComponentCache)), Transient, ConstructorResolutionPolicy(null)]
	public class StaticComponentCache : IComponentCache
	{
		private static readonly ConcurrentDictionary<IComponentRegistration, object> CacheContent =
			new ConcurrentDictionary<IComponentRegistration, object>();

		[CompositionConstructor]
		public StaticComponentCache()
		{
		}

		public object GetComponent(ContractIdentity contract, IComponentRegistration registration, IComposer dependencyResolver)
		{
			return CacheContent.GetOrAdd(registration, r => r.CreateComponent(contract, dependencyResolver));
		}
	}
}