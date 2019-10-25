using System;
using System.Linq;
using ComposerCore.Cache;
using ComposerCore.Extensibility;
using ComposerCore.Utility;

namespace ComposerCore.Implementation
{
    public class ConcreteTypeRegistration : ComponentBuilderRegistration
    {
        public ConcreteTypeRegistration(Type targetType) : base(targetType)
        {
            if (targetType.IsOpenGenericType())
                throw new InvalidOperationException("This class is not designed for open generic types. Use " +
                                                    "GenericComponentRegistration instead.");
        }

        public ConcreteTypeRegistration(Type targetType, string componentCacheName)
            : this(targetType)
        {
            SetCache(componentCacheName);
        }
        
        public ConcreteTypeRegistration(Type targetType, IComponentCache cache)
            : this(targetType)
        {
            SetCache(cache);
        }

        public override object GetComponent(ContractIdentity contract, IComposer dependencyResolver)
        {
            FillCache(dependencyResolver);
            return Cache.GetComponent(contract, this, dependencyResolver);
        }

        public override object CreateComponent(ContractIdentity contract, IComposer dependencyResolver)
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

            var listenerChain = RegistrationContext.GetComponent<ICompositionListenerChain>();
            var componentInstance = listenerChain.NotifyCreated(originalComponentInstance, contract, null, TargetType);

            // Complete the object initialization by applying the initialization
            // points. They should be applied to the original component instance,
            // as the reference may have been changed by composition listeners to
            // an instance that does not have the original configuration points.

//			var initializationPointResults = Initializer.Apply(originalComponentInstance, Composer);
            Initializer.Apply(originalComponentInstance, dependencyResolver);

            // Inform all composition listeners of the newly composed
            // component instance by calling OnComponentComposed method.

//			listenerChain.NotifyComposed(
//				componentInstance, originalComponentInstance, initializationPointResults, contract, _initializationPoints, TargetType);

            // The composition is now finished for the component instance.
            // See if an [OnCompositionComplete] method is specified, call it.
            // This should be called on the original component instance
            // for the same reason stated above.

//			InvokeCompositionNotifications(componentInstance);
            return componentInstance;
        }

        private void FillCacheQuery()
        {
            if (CacheQuery != null)
                return;
            
            // For backward compatibility, the default cache type is set to DefaultComponentCache when there
            // is no [ComponentCache] attribute present on the component.

            var attribute = ComponentContextUtils.GetComponentCacheAttribute(TargetType);
            var name = attribute == null ? nameof(DefaultComponentCache) : attribute.ComponentCacheName;
            
            SetCache(name);
        }

        private void FillCache(IComposer dependencyResolver)
        {
            if (Cache != null)
                return;
            
            FillCacheQuery();
            Cache ??= CacheQuery.Query(dependencyResolver) as IComponentCache
                      ?? throw new CompositionException($"Could not resolve cache component for type {TargetType}.");
        }
    }
}