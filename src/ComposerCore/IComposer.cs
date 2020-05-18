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
	public interface IComposer
	{
        ComposerConfiguration Configuration { get; }

		// Component Lookup methods

		bool IsResolvable<TContract>(string name = null) where TContract : class;
		bool IsResolvable(Type contract, string name = null);
		TContract GetComponent<TContract>(string name = null) where TContract : class;
		object GetComponent(Type contract, string name = null);

		IEnumerable<TContract> GetAllComponents<TContract>(string name = null) where TContract : class;
		IEnumerable<object> GetAllComponents(Type contract, string name = null);

		IEnumerable<TContract> GetComponentFamily<TContract>();
		IEnumerable<object> GetComponentFamily(Type contract);

		// Variable Lookup methods

		bool HasVariable(string name);
		object GetVariable(string name);

		// Other methods

		void InitializePlugs<T>(T componentInstance);
		void InitializePlugs(object componentInstance, Type componentType);
	}
}