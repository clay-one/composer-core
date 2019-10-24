using System;
using System.Collections.Generic;
using System.Linq;
using ComposerCore.Extensibility;
using ComposerCore.Factories;
using ComposerCore.Implementation;
using ComposerCore.Utility;

namespace ComposerCore
{
    public static class ComponentContextExtensions
    {
        public static void Register(this IComponentContext context, Type contract, Type component)
        {
            if (component == null)
                throw new ArgumentNullException(nameof(component));

            var registration = CreateRegistration(component);
            registration.AddContractType(contract);
            
            context.Register(registration);
        }

        public static void Register(this IComponentContext context, Type component)
        {
            if (component == null)
                throw new ArgumentNullException(nameof(component));

            context.Register(CreateRegistration(component));
        }

        public static void Register<TContract, TComponent>(this IComponentContext context, string name = null)
        {
            context.Register(typeof(TContract), name, typeof(TComponent));
        }
        
        public static void Register(this IComponentContext context, Type contract, string name, Type component)
        {
            if (contract == null)
                throw new ArgumentNullException(nameof(contract));
            if (component == null)
                throw new ArgumentNullException(nameof(component));

            var registration = CreateRegistration(component);
            registration.AddContract(new ContractIdentity(contract, name));
            
            context.Register(registration);
        }
        
        public static void Register(this IComponentContext context, IComponentFactory factory)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));
            
            context.Register(new ComponentFactoryRegistration(factory));
        }

        public static void Register(this IComponentContext context, string name, IComponentFactory factory)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));
            
            var registration = new ComponentFactoryRegistration(factory);
            registration.SetDefaultContractName(name);
            
            context.Register(registration);
        }

        public static void Register<TComponent>(this IComponentContext context, string name = null)
        {
            context.Register(name, typeof(TComponent));
        }
        
        public static void Register(this IComponentContext context, string name, Type componentType)
        {
            if (componentType == null)
                throw new ArgumentNullException(nameof(componentType));

            var registration = CreateRegistration(componentType);
            registration.SetDefaultContractName(name);
            
            context.Register(registration);
        }

        public static void Register<TContract>(this IComponentContext context, IComponentFactory factory)
        {
            context.Register(typeof(TContract), factory);
        }
        
        public static void Register(this IComponentContext context, Type contract, IComponentFactory factory)
        {
            if (contract == null)
                throw new ArgumentNullException(nameof(contract));
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));
            
            var registration = new ComponentFactoryRegistration(factory);
            registration.AddContractType(contract);
            
            context.Register(registration);
        }

        public static void Register(this IComponentContext context, Type contract, string name, IComponentFactory factory)
        {
            if (contract == null)
                throw new ArgumentNullException(nameof(contract));
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));
            
            var registration = new ComponentFactoryRegistration(factory);
            registration.AddContract(new ContractIdentity(contract, name));
            
            context.Register(registration);
        }

        public static void RegisterObject(this IComponentContext context, object componentInstance)
        {
            if (componentInstance == null)
                throw new ArgumentNullException(nameof(componentInstance));

            context.Register(new PreInitializedComponentRegistration(componentInstance));
        }

        public static void RegisterObject<TContract>(this IComponentContext context, object componentInstance)
        {
            context.RegisterObject(typeof(TContract), componentInstance);
        }
        
        public static void RegisterObject(this IComponentContext context, Type contract, object componentInstance)
        {
            if (contract == null)
                throw new ArgumentNullException(nameof(contract));
            if (componentInstance == null)
                throw new ArgumentNullException(nameof(componentInstance));

            
            var registration = new PreInitializedComponentRegistration(componentInstance);
            registration.AddContractType(contract);
            
            context.Register(registration);
        }

        public static void RegisterObject(this IComponentContext context, string name, object componentInstance)
        {
            if (componentInstance == null)
                throw new ArgumentNullException(nameof(componentInstance));
	        
            
            var registration = new PreInitializedComponentRegistration(componentInstance);
            registration.SetDefaultContractName(name);
            
            context.Register(registration);
        }

        public static void RegisterObject<TContract>(this IComponentContext context, string name, object componentInstance)
        {
            context.RegisterObject(typeof(TContract), name, componentInstance);
        }
        
        public static void RegisterObject(this IComponentContext context, Type contract, string name, object componentInstance)
        {
            if (contract == null)
                throw new ArgumentNullException(nameof(contract));
            if (componentInstance == null)
                throw new ArgumentNullException(nameof(componentInstance));

            var registration = new PreInitializedComponentRegistration(componentInstance);
            registration.AddContract(new ContractIdentity(contract, name));
            
            context.Register(registration);
        }

        public static void SetVariableValue(this IComponentContext context, string name, object value)
        {
            if (value == null)
                context.RemoveVariable(name);
            else
                context.SetVariable(name, new Lazy<object>(() => value));
        }

        public static void RegisterCompositionListener(this IComponentContext context, string name, ICompositionListener listener)
        {
            context.GetComponent<CompositionListenerChain>().RegisterCompositionListener(name, listener);
        }

        public static void UnregisterCompositionListener(this IComponentContext context, string name)
        {
            context.GetComponent<CompositionListenerChain>().UnregisterCompositionListener(name);
        }
        
        public static bool IsResolvable<TContract>(this IComposer composer, string name = null) where TContract : class
        {
            return composer.IsResolvable(typeof(TContract), name);
        }
        
        public static TContract GetComponent<TContract>(this IComposer composer, string name = null)
            where TContract : class
        {
            return (TContract) composer.GetComponent(typeof (TContract), name);
        }

        public static IEnumerable<TContract> GetAllComponents<TContract>(this IComposer composer, string name = null)
            where TContract : class
        {
            return composer.GetAllComponents(typeof (TContract), name).Cast<TContract>();
        }

        public static IEnumerable<TContract> GetComponentFamily<TContract>(this IComposer composer)
        {
            return composer.GetComponentFamily(typeof (TContract)).Cast<TContract>();
        }

        public static void InitializePlugs<T>(this IComposer composer, T componentInstance)
        {
            composer.InitializePlugs(componentInstance, typeof (T));
        }

        #region Private helpers

        private static IComponentRegistration CreateRegistration(Type componentType)
        {
            if (componentType.IsOpenGenericType())
                return new GenericComponentRegistration(componentType);
            
            return new ConcreteTypeRegistration(componentType);
        }
        
        #endregion
    }
}