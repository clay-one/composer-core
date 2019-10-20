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

		private readonly List<InitializationPointSpecification> _initializationPoints;
		private List<Action<IComposer, object>> _compositionNotificationMethods;

		#region Constructors

		public FactoryMethodComponentFactory(Func<IComposer, TComponent> factoryMethod)
		{
            _factoryMethod = factoryMethod ?? throw new ArgumentNullException(nameof(factoryMethod));

			_composer = null;
			_initializationPoints = new List<InitializationPointSpecification>();
			_compositionNotificationMethods = null;
		}

		#endregion

		#region IComponentFactory Members

		public Type TargetType => typeof(TComponent);

		public bool ValidateContractType(Type contract)
		{
			return contract.IsAssignableFrom(typeof(TComponent));
		}

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
			CompleteConfiguration();
		}

		public IEnumerable<Type> GetContractTypes()
		{
			return ComponentContextUtils.FindContracts(typeof(TComponent));
		}

		public bool IsResolvable(Type contractType)
		{
			return contractType.IsAssignableFrom(typeof(TComponent));
		}

		public object GetComponentInstance(ContractIdentity contract)
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

		public Func<IComposer, TComponent> FactoryMethod => _factoryMethod;

		public List<InitializationPointSpecification> InitializationPoints
		{
			get
			{
				if (_composer != null)
					throw new InvalidOperationException("Cannot access InitializationPoints when the factory is initialized.");

				return _initializationPoints;
			}
		}

	    public List<Action<IComposer, object>> CompositionNotificationMethods
	    {
	        get
	        {
	            if (_composer != null)
	                throw new InvalidOperationException("Cannot access CompositionNotificationMethods when the factory is initialized.");

	            return _compositionNotificationMethods ?? (_compositionNotificationMethods = new List<Action<IComposer, object>>());
	        }
        }

	    #endregion

		#region Private helper methods

		private void CompleteConfiguration()
		{
		    try
		    {
		        LoadInitializationPoints();
		        LoadCompositionNotificationMethods();
		    }
		    catch(Exception e)
		    {
		        throw new CompositionException("Could not initialize DelegateComponentFactory", e);
		    }
		}

		private void LoadInitializationPoints()
		{
			// Check two categories of members for being an initialization point:
			//   1. Public fields
			//   2. Public properties
			// Check and add them to the list of initialization points if they
			// are not already registered.

			foreach (var fieldInfo in typeof(TComponent).GetFields())
			{
				ComponentContextUtils.CheckAndAddInitializationPoint(_composer, _initializationPoints, fieldInfo);
			}

			foreach (var fieldInfo in typeof(TComponent).GetProperties())
			{
				ComponentContextUtils.CheckAndAddInitializationPoint(_composer, _initializationPoints, fieldInfo);
			}
		}

		private void LoadCompositionNotificationMethods()
		{
		    var methodsFound = ComponentContextUtils.FindCompositionNotificationMethods(typeof(TComponent)).ToList();
		    _compositionNotificationMethods = _compositionNotificationMethods?.Concat(methodsFound).ToList() ?? methodsFound;
		}

		private void InvokeCompositionNotifications(object componentInstance)
		{
			if (_compositionNotificationMethods == null)
				return;

			foreach (var method in _compositionNotificationMethods)
			{
			    method(_composer, componentInstance);
			}
		}

		private object CreateComponent(ContractIdentity contract, ICompositionListenerChain listenerChain)
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

			var componentInstance = listenerChain.NotifyCreated(originalComponentInstance, contract, this, typeof(TComponent));

		    // Complete the object initialization by applying the initialization
			// points. They should be applied to the original component instance,
			// as the reference may have been changed by composition listeners to
			// an instance that does not have the original configuration points.

			var initializationPointResults = ApplyInitializationPoints(originalComponentInstance);

			// Inform all composition listeners of the newly composed
			// component instance by calling OnComponentComposed method.

			listenerChain.NotifyComposed(componentInstance, originalComponentInstance, initializationPointResults, contract, _initializationPoints, typeof(TComponent));

			// The composition is now finished for the component instance.
			// See if an [OnCompositionComplete] method is specified, call it.
			// This should be called on the original component instance
			// for the same reason stated above.

			InvokeCompositionNotifications(componentInstance);
			return componentInstance;
		}
		
		private List<object> ApplyInitializationPoints(object originalComponentInstance)
		{
			var initializationPointResults = new List<object>();

			foreach (var initializationPoint in _initializationPoints)
			{
				if (initializationPoint.Query == null)
					throw new CompositionException(
					        $"Query is null for initialization point '{initializationPoint.Name}' on component instance of type '{originalComponentInstance.GetType().FullName}'");

				var initializationPointResult = initializationPoint.Query.Query(_composer);

				// Check if the required initialization points get a value.
				if (initializationPoint.Required.GetValueOrDefault(_composer.Configuration.InitializationPointsRequiredByDefault) && initializationPointResult == null)
					throw new CompositionException(
					        $"Could not fill initialization point '{initializationPoint.Name}' of type '{originalComponentInstance.GetType().FullName}'.");

				initializationPointResults.Add(initializationPointResult);
				ComponentContextUtils.ApplyInitializationPoint(originalComponentInstance,
				                                               initializationPoint.Name,
				                                               initializationPoint.MemberType,
				                                               initializationPointResult);
			}

			return initializationPointResults;
		}


		#endregion
	}
}