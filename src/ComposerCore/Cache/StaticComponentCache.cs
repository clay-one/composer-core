using System;
using System.Collections.Concurrent;
using ComposerCore.Attributes;
using ComposerCore.Extensibility;

namespace ComposerCore.Cache
{
	[Component(nameof(StaticComponentCache)), Transient, ConstructorResolutionPolicy(null)]
	public class StaticComponentCache : IComponentCache
	{
		private static readonly ConcurrentDictionary<IComponentRegistration, object> CacheContent =
			new ConcurrentDictionary<IComponentRegistration, object>();

		private readonly IComposer _composer;

		[CompositionConstructor]
		public StaticComponentCache(IComposer composer)
		{
			_composer = composer ?? throw new ArgumentNullException(nameof(composer));
		}

		public object GetComponent(ContractIdentity contract, IComponentRegistration registration, IComposer scope)
		{
			return CacheContent.GetOrAdd(registration, r => r.CreateComponent(contract, scope));
		}

		public void Dispose()
		{
			throw new NotImplementedException();
		}
	}
}