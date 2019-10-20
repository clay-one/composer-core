using System;
using System.Collections.Generic;
using System.Linq;

namespace ComposerCore.Implementation
{
	internal class ComponentRepository
	{
		private readonly IDictionary<ContractIdentity, List<ComponentRegistration>> _registrationMap;

		public ComponentRepository()
		{
			_registrationMap = new Dictionary<ContractIdentity, List<ComponentRegistration>>();
		}

		public void Add(ComponentRegistration registration)
		{
			foreach (var contract in registration.Contracts)
			{
				if (!_registrationMap.ContainsKey(contract))
					_registrationMap.Add(contract, new List<ComponentRegistration>());

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

		public IEnumerable<ComponentRegistration> FindFactories(ContractIdentity identity)
		{
		    _registrationMap.TryGetValue(identity, out var closedResults);
		    if (!identity.Type.IsGenericType)
                return closedResults ?? Enumerable.Empty<ComponentRegistration>();

		    var genericContractType = identity.Type.GetGenericTypeDefinition();
		    var genericIdentity = new ContractIdentity(genericContractType, identity.Name);

		    if (_registrationMap.TryGetValue(genericIdentity, out var genericResults))
		    {
		        return closedResults?.Concat(genericResults) ?? genericResults;
		    }

		    return closedResults ?? Enumerable.Empty<ComponentRegistration>();
		}

		public IEnumerable<ContractIdentity> GetContractIdentityFamily(Type type)
		{
			return _registrationMap.Keys.Where(i => i.Type == type);
		}
	}
}