using System;
using System.Collections.Generic;
using ComposerCore.Extensibility;
using ComposerCore.Implementation;


namespace ComposerCore.Factories
{
	public class LocalComponentFactory : LocalComponentFactoryBase
	{
		#region Constructors

		public LocalComponentFactory(Type targetType, LocalComponentFactoryBase original = null)
			: base(targetType, original)
		{
		}

		#endregion

		#region IComponentFactory Members

		public override bool ValidateContractType(Type contract)
		{
			return contract.IsAssignableFrom(TargetType);
		}

		public override void Initialize(IComposer composer)
		{
			base.Initialize(composer);

			try
			{
				LoadInitializationPoints();
				LoadCompositionNotificationMethods();
			}
			catch(Exception e)
			{
				throw new CompositionException(
					$"Could not initialize LocalComponentFactory for type '{TargetType.FullName}'", e);
			}
		}

		public override IEnumerable<Type> GetContractTypes()
		{
			return ComponentContextUtils.FindContracts(TargetType);
		}

		public override bool IsResolvable(Type contractType)
		{
			return contractType.IsAssignableFrom(TargetType);
		}

		public override object GetComponentInstance(ContractIdentity contract)
		{
			if (!Initialized)
				throw new InvalidOperationException(
					"LocalComponentFactory should be initialized before calling GetComponentInstance method.");

			var listenerChain = Composer.GetComponent<ICompositionListenerChain>();
			return CreateComponent(contract, listenerChain);
		}

		#endregion
		
		#region Private helper methods
		
		private object CreateComponent(ContractIdentity contract, ICompositionListenerChain listenerChain)
		{
			// Save the original component instance reference, so that
			// we can apply initialization points to it later, as the
			// composition listeners may change the reference to a
			// wrapped one.

			var originalComponentInstance = Builder.Build();

			// After constructing the component object, first process
			// all composition listeners so that if the reference should
			// change, it changes before setting it to the cache.
			// Otherwise, in circular dependency scenarios, dependent
			// components may get unwrapped component where the component
			// is wrapped by composition listeners.

			var componentInstance = listenerChain.NotifyCreated(originalComponentInstance, contract, this, TargetType);

		    // Complete the object initialization by applying the initialization
			// points. They should be applied to the original component instance,
			// as the reference may have been changed by composition listeners to
			// an instance that does not have the original configuration points.

			var initializationPointResults = ApplyInitializationPoints(originalComponentInstance);

			// Inform all composition listeners of the newly composed
			// component instance by calling OnComponentComposed method.

			listenerChain.NotifyComposed(
				componentInstance, originalComponentInstance, initializationPointResults, contract, _initializationPoints, TargetType);

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
					        $"Query is null for initialization point '{initializationPoint.Name}' on component instance of type '{TargetType.FullName}'");

				if (initializationPoint.Query.IsResolvable(Composer))
				{
					var initializationPointResult = initializationPoint.Query.Query(Composer);

					initializationPointResults.Add(initializationPointResult);
					ComponentContextUtils.ApplyInitializationPoint(originalComponentInstance,
						initializationPoint.Name,
						initializationPoint.MemberType,
						initializationPointResult);
				}
				else
				{
					// Check if the required initialization points get a value.
					if (initializationPoint.Required.GetValueOrDefault(Composer.Configuration.InitializationPointsRequiredByDefault))
						throw new CompositionException(
							$"Could not fill initialization point '{initializationPoint.Name}' of type '{TargetType.FullName}'.");
					
					initializationPointResults.Add(null);
				}
				
			}

			return initializationPointResults;
		}

		#endregion
	}
}