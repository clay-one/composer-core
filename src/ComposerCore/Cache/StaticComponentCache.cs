using System.Collections.Concurrent;
using ComposerCore.Attributes;
using ComposerCore.Extensibility;
using ComposerCore.Implementation;

namespace ComposerCore.Cache
{
	[Component(nameof(StaticComponentCache)), Transient, ConstructorResolutionPolicy(null)]
	public class StaticComponentCache : IComponentCache
	{
		private static readonly ConcurrentDictionary<ComponentRegistration, object> CacheContent =
			new ConcurrentDictionary<ComponentRegistration, object>();

		[CompositionConstructor]
		public StaticComponentCache()
		{
		}

		public object GetComponent(ContractIdentity contract, ComponentRegistration registration, IComposer dependencyResolver)
		{
			return CacheContent.GetOrAdd(registration, r => r.Factory.GetComponentInstance(contract));
		}
	}
}