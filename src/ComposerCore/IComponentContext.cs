using System;
using ComposerCore.Attributes;
using ComposerCore.Extensibility;
using ComposerCore.Implementation;

namespace ComposerCore
{
	[Contract]
	public interface IComponentContext : IComposer
	{
		void Register(ComponentRegistration componentRegistration);

		void Unregister(ContractIdentity identity);
		void UnregisterFamily(Type type);

		void SetVariable(string name, Lazy<object> value);
		void RemoveVariable(string name);
	}
}
