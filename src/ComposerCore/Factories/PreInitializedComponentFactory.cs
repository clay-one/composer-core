using System;
using System.Collections.Generic;
using ComposerCore.Extensibility;
using ComposerCore.Implementation;

namespace ComposerCore.Factories
{
	[Obsolete]
	public class PreInitializedComponentFactory : IComponentFactory
	{
		private readonly object _componentInstance;

		public PreInitializedComponentFactory(object componentInstance)
		{
		    _componentInstance = componentInstance ?? throw new ArgumentNullException(nameof(componentInstance));
		}

		public Type TargetType => _componentInstance.GetType();

		public void Initialize(IComposer composer)
		{
		}

		public IEnumerable<Type> GetContractTypes()
		{
			return ComponentContextUtils.FindContracts(_componentInstance.GetType());
		}

	    public object GetComponentInstance(ContractIdentity contract)
		{
			return _componentInstance;
		}
	}
}