using System;
using System.Collections.Generic;
using ComposerCore.Attributes;

namespace ComposerCore
{
	/// <summary>
	/// Specifies the interface using which the code can query the Composer and
	/// ask for component instances.
	/// </summary>
	[Contract]
	public interface IComposer : IDisposable
	{
        ComposerConfiguration Configuration { get; }

		bool IsResolvable(Type contract, string name = null);
		object GetComponent(Type contract, string name = null);
		IEnumerable<object> GetAllComponents(Type contract, string name = null);
		IEnumerable<object> GetComponentFamily(Type contract);

		bool HasVariable(string name);
		object GetVariable(string name);

		void InitializePlugs(object componentInstance, Type componentType);
	}
}