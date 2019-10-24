using System;
using System.Collections.Generic;
using System.Linq;
using ComposerCore.Extensibility;
using ComposerCore.Implementation;

namespace ComposerCore.Factories
{
	public class FactoryMethodComponentFactory<TComponent> : IComponentFactory where TComponent : class
	{
		private IComposer _composer;

		private readonly Func<IComposer, TComponent> _factoryMethod;

		#region Constructors

		public FactoryMethodComponentFactory(Func<IComposer, TComponent> factoryMethod)
		{
            _factoryMethod = factoryMethod ?? throw new ArgumentNullException(nameof(factoryMethod));
			_composer = null;
		}

		#endregion

		#region IComponentFactory Members

		public Type TargetType => typeof(TComponent);

		public void Initialize(IComposer composer)
		{
			if (_composer != null)
				return;

			if (_factoryMethod == null)
				throw new InvalidOperationException("FactoryMethod is not specified.");

			if (!composer.Configuration.DisableAttributeChecking && !ComponentContextUtils.HasComponentAttribute(typeof(TComponent)))
				throw new CompositionException("The type '" + typeof(TComponent) +
				                               "' is not a component, but it is being registered as one. Only classes marked with [Component] attribute can be registered.");

			_composer = composer;
		}

		public IEnumerable<Type> GetContractTypes()
		{
			return ComponentContextUtils.FindContracts(typeof(TComponent));
		}

		public object GetComponentInstance(ContractIdentity contract)
		{
			// Check if the factory is initialized

			if (_composer == null)
				throw new InvalidOperationException(
					"DelegateComponentFactory should be initialized before calling GetComponentInstance method.");

			return CreateComponent(contract);
		}

		#endregion

		#region Override methods

		public override string ToString()
		{
			return _factoryMethod?.ToString() ?? base.ToString();
		}

		#endregion

		#region Private helper methods
		
		private object CreateComponent(ContractIdentity contract)
		{
			// Save the original component instance reference, so that
			// we can apply initialization points to it later, as the
			// composition listeners may change the reference to a
			// wrapped one.

			object originalComponentInstance = _factoryMethod(_composer);

			// After constructing the component object, first process
			// all composition listeners so that if the reference should
			// change, it changes before setting it to the cache.
			// Otherwise, in circular dependency scenarios, dependent
			// components may get unwrapped component where the component
			// is wrapped by composition listeners.

//			var componentInstance = listenerChain.NotifyCreated(originalComponentInstance, contract, this, typeof(TComponent));

		    // Complete the object initialization by applying the initialization
			// points. They should be applied to the original component instance,
			// as the reference may have been changed by composition listeners to
			// an instance that does not have the original configuration points.

//			var initializationPointResults = ApplyInitializationPoints(originalComponentInstance);

			// Inform all composition listeners of the newly composed
			// component instance by calling OnComponentComposed method.

//			listenerChain.NotifyComposed(componentInstance, originalComponentInstance, initializationPointResults, contract, _initializationPoints, typeof(TComponent));

			// The composition is now finished for the component instance.
			// See if an [OnCompositionComplete] method is specified, call it.
			// This should be called on the original component instance
			// for the same reason stated above.

//			InvokeCompositionNotifications(componentInstance);
			return originalComponentInstance;
		}

		#endregion
	}
}