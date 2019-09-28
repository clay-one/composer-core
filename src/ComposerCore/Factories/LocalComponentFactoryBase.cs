using System;
using System.Collections.Generic;
using ComposerCore.Extensibility;
using ComposerCore.Implementation;

namespace ComposerCore.Factories
{
    public abstract class LocalComponentFactoryBase : IComponentFactory
    {
        protected IComposer Composer { get; private set; }
        protected List<ConstructorArgSpecification> _constructorArgs;
        protected List<InitializationPointSpecification> _initializationPoints;
        protected ICompositionalQuery _componentCacheQuery;
        protected List<Action<IComposer, object>> _compositionNotificationMethods;
        
        public Type TargetType { get; }
        public bool Initialized => Composer != null;


        protected LocalComponentFactoryBase(Type targetType)
        {
            TargetType = targetType ?? throw new ArgumentNullException(nameof(targetType));
            
            Composer = null;
            _constructorArgs = null;
            _initializationPoints = null;
            _componentCacheQuery = null;
            _compositionNotificationMethods = null;
        }

        #region IComponentFactory

        public virtual bool ValidateContractType(Type contract)
        {
            return true;
        }

        public virtual void Initialize(IComposer composer)
        {
            if (Initialized)
                return;
            
            if (TargetType == null)
                throw new InvalidOperationException("TargetType is not specified.");

            if (!composer.Configuration.DisableAttributeChecking && !ComponentContextUtils.HasComponentAttribute(TargetType))
                throw new CompositionException("The type '" + TargetType +
                                               "' is not a component, but it is being registered as one. Only classes marked with [Component] attribute can be registered.");
            Composer = composer;
        }
        
        public abstract IEnumerable<Type> GetContractTypes();
        public abstract object GetComponentInstance(ContractIdentity contract, IEnumerable<ICompositionListener> listenerChain);

        #endregion

        public override string ToString()
        {
            return TargetType != null ? TargetType.AssemblyQualifiedName : base.ToString();
        }

        protected void EnsureNotInitialized(string operation)
        {
            if (Initialized)
                throw new InvalidOperationException($"Cannot perform operation '{operation}' when the factory is not initialized.");
        }
        
        protected void InvokeCompositionNotifications(object componentInstance)
        {
            if (_compositionNotificationMethods == null)
                return;

            foreach (var method in _compositionNotificationMethods)
            {
                method(Composer, componentInstance);
            }
        }

        public List<ConstructorArgSpecification> ConstructorArgs
        {
            get
            {
                EnsureNotInitialized("access ConstructorArgs");
                return _constructorArgs ?? (_constructorArgs = new List<ConstructorArgSpecification>());
            }
        }

        public List<InitializationPointSpecification> InitializationPoints
        {
            get
            {
                EnsureNotInitialized("access InitializationPoints");
                return _initializationPoints ?? (_initializationPoints = new List<InitializationPointSpecification>());
            }
        }
        
        public ICompositionalQuery ComponentCacheQuery
        {
            get
            {
                EnsureNotInitialized("access ComponentCacheQuery");
                return _componentCacheQuery;
            }
            set
            {
                EnsureNotInitialized("access ComponentCacheQuery");
                _componentCacheQuery = value;
            }
        }
        
        public List<Action<IComposer, object>> CompositionNotificationMethods
        {
            get
            {
                EnsureNotInitialized("access CompositionNotificationMethods");
                return _compositionNotificationMethods ?? (_compositionNotificationMethods = new List<Action<IComposer, object>>());
            }
        }


    }
}