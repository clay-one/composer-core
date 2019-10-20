using System;
using System.Collections.Generic;

namespace ComposerCore.Extensibility
{
	/// <summary>
	/// Specifies the interface required by factories that actually create the component instance.
	/// </summary>
	public interface IComponentFactory
	{
		Type TargetType { get; }
		
		bool ValidateContractType(Type contract);
		void Initialize(IComposer composer);
		IEnumerable<Type> GetContractTypes();

		bool IsResolvable(Type contractType);
		object GetComponentInstance(ContractIdentity contract);
	}
}