using System.Collections.Concurrent;
using ComposerCore.Attributes;
using ComposerCore.Extensibility;
using ComposerCore.Implementation;

namespace ComposerCore.Cache
{
	[Component(nameof(StaticComponentCache)), Transient, ConstructorResolutionPolicy(null)]
	public class StaticComponentCache : IComponentCache
	{
		private static readonly ConcurrentDictionary<ConcreteComponentRegistration, object> CacheContent =
			new ConcurrentDictionary<ConcreteComponentRegistration, object>();

		[CompositionConstructor]
		public StaticComponentCache()
		{
		}

		public object GetComponent(ContractIdentity contract, ConcreteComponentRegistration registration, IComposer dependencyResolver)
		{
			return CacheContent.GetOrAdd(registration, r => r.Factory.GetComponentInstance(contract));
		}
	}
}