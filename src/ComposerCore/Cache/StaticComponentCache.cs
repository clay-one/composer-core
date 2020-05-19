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

		[CompositionConstructor]
		public StaticComponentCache()
		{
		}

		public object GetComponent(ContractIdentity contract, IComponentRegistration registration, IComposer scope)
		{
			return CacheContent.GetOrAdd(registration, r =>
			{
				var component = r.CreateComponent(contract, scope);
				if (component is IDisposable disposable)
					registration.RegistrationContext.TrackDisposable(disposable);

				return component;
			});
		}
	}
}