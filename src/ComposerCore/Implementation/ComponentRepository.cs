using System;
using System.Collections.Generic;
using System.Linq;
using ComposerCore.Extensibility;

namespace ComposerCore.Implementation
{
	internal class ComponentRepository : IDisposable
	{
		private readonly IDictionary<ContractIdentity, List<IComponentRegistration>> _registrationMap;
		private readonly List<WeakReference<IDisposable>> _recycleBin;
		private volatile bool _disposed;

		public ComponentRepository()
		{
			_registrationMap = new Dictionary<ContractIdentity, List<IComponentRegistration>>();
			_recycleBin = new List<WeakReference<IDisposable>>();
			_disposed = false;
		}

		public void Add(IComponentRegistration registration)
		{
			foreach (var contract in registration.Contracts)
			{
				if (!_registrationMap.ContainsKey(contract))
					_registrationMap.Add(contract, new List<IComponentRegistration>());

				_registrationMap[contract].Add(registration);
			}
		}

		public void Remove(ContractIdentity identity)
		{
			if (_registrationMap.ContainsKey(identity))
				_registrationMap.Remove(identity);
		}

		public void RemoveAll(Type type)
		{
			var identitiesToRemove = _registrationMap.Keys.Where(i => i.Type == type).ToArray();
			Array.ForEach(identitiesToRemove, i => _registrationMap.Remove(i));
		}

		public IEnumerable<IComponentRegistration> Find(ContractIdentity identity)
		{
		    _registrationMap.TryGetValue(identity, out var closedResults);
		    if (!identity.Type.IsGenericType)
                return closedResults ?? Enumerable.Empty<IComponentRegistration>();

		    var genericContractType = identity.Type.GetGenericTypeDefinition();
		    var genericIdentity = new ContractIdentity(genericContractType, identity.Name);

		    if (_registrationMap.TryGetValue(genericIdentity, out var genericResults))
		    {
		        return closedResults?.Concat(genericResults) ?? genericResults;
		    }

		    return closedResults ?? Enumerable.Empty<IComponentRegistration>();
		}

		public IEnumerable<ContractIdentity> GetContractIdentityFamily(Type type)
		{
			return _registrationMap.Keys.Where(i => i.Type == type);
		}

		public void Dispose()
		{
			if (_disposed)
				return;

			var disposables = _recycleBin
				.Select(wr => wr.TryGetTarget(out var d) ? d : null)
				.Where(d => d != null)
				.Distinct();
			
			foreach (var disposable in disposables)
			{
				disposable?.Dispose();
			}
		}

		public void AddToRecycleBin(IDisposable disposable)
		{
			_recycleBin.Add(new WeakReference<IDisposable>(disposable));
		}
	}
}