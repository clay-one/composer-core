using System;
using System.Collections.Generic;
using ComposerCore.Attributes;
using ComposerCore.Extensibility;
using ComposerCore.Implementation;


namespace ComposerCore.Factories
{
	public class LocalComponentFactory : IComponentFactory
	{
		public Type TargetType { get; }
		protected IComposer Composer { get; private set; }
		protected LocalComponentBuilder Builder { get; }
        
		public bool Initialized => Composer != null;

		public ConstructorResolutionPolicy? ConstructorResolutionPolicy
		{
			get => Builder.ConstructorResolutionPolicy;
			set => Builder.ConstructorResolutionPolicy = value;
		}
		
		public LocalComponentFactory(Type targetType, LocalComponentFactory original = null)
		{
			TargetType = targetType ?? throw new ArgumentNullException(nameof(targetType));
            
			Composer = null;
			Builder = new LocalComponentBuilder(targetType, original?.Builder);
		}

		#region IComponentFactory Members

		public void Initialize(IComposer composer)
		{
			if (Initialized)
				return;

			if (TargetType == null)
				throw new InvalidOperationException("TargetType is not specified.");

			if (!composer.Configuration.DisableAttributeChecking && !ComponentContextUtils.HasComponentAttribute(TargetType))
				throw new CompositionException("The type '" + TargetType +
				                               "' is not a component, but it is being registered as one. Only classes marked with [Component] attribute can be registered.");
			Builder.Initialize(composer);
			Composer = composer;
		}

		public IEnumerable<Type> GetContractTypes()
		{
			return ComponentContextUtils.FindContracts(TargetType);
		}

		public object GetComponentInstance(ContractIdentity contract)
		{
			if (!Initialized)
				throw new InvalidOperationException(
					"LocalComponentFactory should be initialized before calling GetComponentInstance method.");

			return CreateComponent(contract);
		}

		#endregion
		
		#region Private helper methods
		
		private object CreateComponent(ContractIdentity contract)
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

//			var componentInstance = listenerChain.NotifyCreated(originalComponentInstance, contract, this, TargetType);

		    // Complete the object initialization by applying the initialization
			// points. They should be applied to the original component instance,
			// as the reference may have been changed by composition listeners to
			// an instance that does not have the original configuration points.

//			var initializationPointResults = Initializer.Apply(originalComponentInstance, Composer);
//			Initializer.Apply(originalComponentInstance, Composer);

			// Inform all composition listeners of the newly composed
			// component instance by calling OnComponentComposed method.

//			listenerChain.NotifyComposed(
//				componentInstance, originalComponentInstance, initializationPointResults, contract, _initializationPoints, TargetType);

			// The composition is now finished for the component instance.
			// See if an [OnCompositionComplete] method is specified, call it.
			// This should be called on the original component instance
			// for the same reason stated above.

//			InvokeCompositionNotifications(componentInstance);
			return originalComponentInstance;
		}
		
		public void AddConfiguredConstructorArg(ConstructorArgSpecification cas)
		{
			Builder.AddConfiguredConstructorArg(cas);
		}


		public override string ToString()
		{
			return TargetType?.AssemblyQualifiedName ?? base.ToString();
		}

		protected void EnsureNotInitialized()
		{
			if (Initialized)
				throw new InvalidOperationException($"Cannot perform operation when the factory is not initialized.");
		}

		#endregion
	}
}