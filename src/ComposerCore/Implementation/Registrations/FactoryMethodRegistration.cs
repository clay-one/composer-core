using System;
using ComposerCore.Cache;
using ComposerCore.Extensibility;

namespace ComposerCore.Implementation
{
    public class FactoryMethodRegistration<TComponent> : ComponentInitializerRegistration
    {
        private readonly Func<IComposer, TComponent> _factoryMethod;

        public FactoryMethodRegistration(Func<IComposer, TComponent> factoryMethod) : base(typeof(TComponent))
        {
            _factoryMethod = factoryMethod;
        }
        
        public override object GetComponent(ContractIdentity contract, IComposer scope)
        {
            FillCache();
            return Cache.GetComponent(contract, this, scope);
        }

        public override object CreateComponent(ContractIdentity contract, IComposer scope)
        {
            // Save the original component instance reference, so that
            // we can apply initialization points to it later, as the
            // composition listeners may change the reference to a
            // wrapped one.

            var originalComponentInstance = _factoryMethod(RegistrationContext);

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
            Initializer.Apply(originalComponentInstance, scope);

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

        private void FillCache()
        {
            if (Cache != null)
                return;
            
            FillCacheQuery();
            Cache ??= CacheQuery.Query(RegistrationContext) as IComponentCache
                      ?? throw new CompositionException($"Could not resolve cache component for type {TargetType}.");
        }
    }
}