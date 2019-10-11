using System;
using System.Collections.Generic;
using System.Linq;
using ComposerCore.Attributes;
using ComposerCore.Cache;
using ComposerCore.CompositionalQueries;
using ComposerCore.Extensibility;
using ComposerCore.Implementation;

namespace ComposerCore.Factories
{
    public abstract class LocalComponentFactoryBase : IComponentFactory
    {
        protected IComposer Composer { get; private set; }
        protected LocalComponentBuilder Builder { get; }
        
        public Type TargetType { get; }
        public bool Initialized => Composer != null;

        public ConstructorResolutionPolicy? ConstructorResolutionPolicy
        {
            get => Builder.ConstructorResolutionPolicy;
            set => Builder.ConstructorResolutionPolicy = value;
        }

        protected LocalComponentFactoryBase(Type targetType, LocalComponentFactoryBase original = null)
        {
            TargetType = targetType ?? throw new ArgumentNullException(nameof(targetType));
            
            Composer = null;
            Builder = new LocalComponentBuilder(targetType, original?.Builder);
            
            _initializationPoints = original?._initializationPoints == null ? null : new List<InitializationPointSpecification>(original._initializationPoints);
            _componentCacheQuery = original?._componentCacheQuery;
            _compositionNotificationMethods = original?._compositionNotificationMethods == null ? null : new List<Action<IComposer, object>>(original._compositionNotificationMethods);
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
            Builder.Initialize(composer);
            Composer = composer;
        }
        
        public abstract IEnumerable<Type> GetContractTypes();
        public abstract bool IsResolvable(Type contractType);
        public abstract object GetComponentInstance(ContractIdentity contract);

        #endregion

        public void AddConfiguredConstructorArg(ConstructorArgSpecification cas)
        {
            Builder.AddConfiguredConstructorArg(cas);
        }
        
        #region InitializationPoints
        
        protected List<InitializationPointSpecification> _initializationPoints;

        protected void LoadInitializationPoints()
        {
            // Check two categories of members for being an initialization point:
            //   1. Public fields
            //   2. Public properties
            // Check and add them to the list of initialization points if they
            // are not already registered.

            _initializationPoints = _initializationPoints ?? new List<InitializationPointSpecification>();
			
            foreach (var fieldInfo in TargetType.GetFields())
            {
                ComponentContextUtils.CheckAndAddInitializationPoint(Composer, _initializationPoints, fieldInfo);
            }

            foreach (var fieldInfo in TargetType.GetProperties())
            {
                ComponentContextUtils.CheckAndAddInitializationPoint(Composer, _initializationPoints, fieldInfo);
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

        #endregion
        
        #region ComponentCacheQuery
        
        protected ICompositionalQuery _componentCacheQuery;

        protected void LoadComponentCacheQuery()
        {
            if (_componentCacheQuery != null)
                return;

            var attribute = ComponentContextUtils.GetComponentCacheAttribute(TargetType);
            if (attribute == null)
            {
                _componentCacheQuery = new ComponentQuery(typeof(DefaultComponentCache), null);
                return;
            }

            if (attribute.ComponentCacheType == null)
            {
                _componentCacheQuery = null;
                return;
            }

            _componentCacheQuery = new ComponentQuery(attribute.ComponentCacheType, attribute.ComponentCacheName);
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

        #endregion
        
        #region CompositionNotification
        
        protected List<Action<IComposer, object>> _compositionNotificationMethods;

        protected void LoadCompositionNotificationMethods()
        {
            var methodsFound = ComponentContextUtils.FindCompositionNotificationMethods(TargetType).ToList();
            _compositionNotificationMethods = _compositionNotificationMethods?.Concat(methodsFound).ToList() ?? methodsFound;
        }
        
        public List<Action<IComposer, object>> CompositionNotificationMethods
        {
            get
            {
                EnsureNotInitialized("access CompositionNotificationMethods");
                return _compositionNotificationMethods ?? (_compositionNotificationMethods = new List<Action<IComposer, object>>());
            }
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

        #endregion

        public override string ToString()
        {
            return TargetType?.AssemblyQualifiedName ?? base.ToString();
        }

        protected void EnsureNotInitialized(string operation)
        {
            if (Initialized)
                throw new InvalidOperationException($"Cannot perform operation '{operation}' when the factory is not initialized.");
        }
        
    }
}