using System;
using System.Collections.Generic;
using System.Linq;
using ComposerCore.Extensibility;
using ComposerCore.Implementation;

namespace ComposerCore.Factories
{
    public class UntypedFactoryMethodComponentFactory : IComponentFactory
    {
		private IComposer _composer;

		private readonly Func<IComposer, object> _factoryMethod;

	    private List<Type> _contractTypes;

		#region Constructors

		public UntypedFactoryMethodComponentFactory(Func<IComposer, object> factoryMethod, IEnumerable<Type> contractTypes = null)
		{
            _factoryMethod = factoryMethod ?? throw new ArgumentNullException(nameof(factoryMethod));

			_composer = null;
            _contractTypes = contractTypes?.ToList();
		}

		#endregion

		#region IComponentFactory Members

		public Type TargetType => null;

		public void Initialize(IComposer composer)
		{
			if (_composer != null)
				return;

			if (_factoryMethod == null)
				throw new InvalidOperationException("FactoryMethod is not specified.");

			_composer = composer;
		}

		public IEnumerable<Type> GetContractTypes()
		{
			if (_contractTypes == null || _contractTypes.Count == 0)
				throw new CompositionException("Contracts are not specified. For an UntypedFactoryMethod, there is " +
				                               "no way of discovering contracts before actual invocation of the " +
				                               "method and instantiation of the component. Contract types should " +
				                               "be provided to the factory using ContractTypes property before " +
				                               "registering it in the ComponentContext.");
			
			return _contractTypes;
		}
		
		public object GetComponentInstance(ContractIdentity contract, IComposer scope)
		{
			// Check if the factory is initialized

			if (_composer == null)
				throw new InvalidOperationException(
					"DelegateComponentFactory should be initialized before calling GetComponentInstance method.");

			var listenerChain = _composer.GetComponent<ICompositionListenerChain>();
			return CreateComponent(contract, listenerChain);
		}

		#endregion

		#region Override methods

		public override string ToString()
		{
			return _factoryMethod?.ToString() ?? base.ToString();
		}

		#endregion

		#region Public properties

		public Func<IComposer, object> FactoryMethod => _factoryMethod;

	    public List<Type> ContractTypes
	    {
	        get
	        {
	            if (_composer != null)
	                throw new InvalidOperationException("Cannot access ComponentCacheQuery when the factory is initialized.");

	            return _contractTypes;
	        }
	        set
	        {
	            if (_composer != null)
	                throw new InvalidOperationException("Cannot access ComponentCacheQuery when the factory is initialized.");

	            _contractTypes = value;
	        }
	    }

	    #endregion

		#region Private helper methods
		
        private object CreateComponent(ContractIdentity contract, ICompositionListenerChain listenerChain)
		{
			// Save the original component instance reference, so that
			// we can apply initialization points to it later, as the
			// composition listeners may change the reference to a
			// wrapped one.

			var originalComponentInstance = _factoryMethod(_composer);

			// After constructing the component object, first process
			// all composition listeners so that if the reference should
			// change, it changes before setting it to the cache.
			// Otherwise, in circular dependency scenarios, dependent
			// components may get unwrapped component where the component
			// is wrapped by composition listeners.

			var componentInstance = listenerChain.NotifyCreated(originalComponentInstance, contract, this, originalComponentInstance.GetType());

		    // Complete the object initialization by applying the initialization
			// points. They should be applied to the original component instance,
			// as the reference may have been changed by composition listeners to
			// an instance that does not have the original configuration points.

			_composer.InitializePlugs(originalComponentInstance, originalComponentInstance.GetType());
			
			// We do not call "OnComponentComposed" on the listener chain here, because they are already called
			// with the "InitializePlugs" call

			return componentInstance;
		}
        
        #endregion
        
    }
}