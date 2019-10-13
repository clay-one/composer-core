﻿using System;
using ComposerCore.Attributes;
using ComposerCore.Extensibility;

namespace ComposerCore
{
	[Contract]
	public interface IComponentContext : IComposer
	{
		void Register(Type contract, string name, IComponentFactory factory);

		void Unregister(ContractIdentity identity);
		void UnregisterFamily(Type type);

		void SetVariable(string name, Lazy<object> value);
		void RemoveVariable(string name);
	}
}
