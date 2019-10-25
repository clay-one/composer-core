using System;
using System.Collections.Generic;
using System.Linq;
using ComposerCore.Implementation;

namespace ComposerCore.Factories
{
    public class LocalComponentInitializer
    {
        private readonly Type _targetType;
        private IComposer _composer;
        private bool Initialized => _composer != null;
        
        private List<InitializationPointSpecification> _configuredInitializationPoints;
        private List<Action<IComposer, object>> _configuredCompositionNotifications;
        
        private List<InitializationPointSpecification> _initializationPoints;
        private List<Action<IComposer, object>> _compositionNotifications;

        public LocalComponentInitializer(Type targetType)
        {
            _targetType = targetType ?? throw new ArgumentNullException(nameof(targetType));

            _configuredInitializationPoints = null;
            _configuredCompositionNotifications = null;

            _initializationPoints = null;
            _compositionNotifications = null;
        }

        public void CopyConfigFrom(LocalComponentInitializer original)
        {
            _configuredInitializationPoints = original?._configuredInitializationPoints == null
                ? null
                : new List<InitializationPointSpecification>(original._configuredInitializationPoints);
            _configuredCompositionNotifications = original?._configuredCompositionNotifications == null
                ? null
                : new List<Action<IComposer, object>>(original._configuredCompositionNotifications);
        }


        public void AddConfiguredInitializationPoint(InitializationPointSpecification specification)
        {
            if (Initialized)
                throw new CompositionException("Cannot change configured initialization points after initialization is completed.");
            
            _configuredInitializationPoints ??= new List<InitializationPointSpecification>();
            _configuredInitializationPoints.Add(specification);
        }

        public void AddCompositionNotification(Action<IComposer,object> initAction)
        {
            if (Initialized)
                throw new CompositionException("Cannot change configured composition notifications after initialization is completed.");
            
            _configuredCompositionNotifications ??= new List<Action<IComposer, object>>();
            _configuredCompositionNotifications.Add(initAction);
        }

        public void Initialize(IComposer composer)
        {
            _composer = composer;
        }

        public void Apply(object originalComponentInstance, IComposer dependencyResolver)
        {
            if (!Initialized)
                throw new InvalidOperationException("LocalComponentInitializer should be Initialized before calling Build");
            
            LoadInitializationPoints();
            LoadCompositionNotificationMethods();

            ApplyInitializationPoints(originalComponentInstance, dependencyResolver);
            InvokeCompositionNotifications(originalComponentInstance, dependencyResolver);
        }
        
        private void LoadInitializationPoints()
        {
            if (_initializationPoints != null)
                return;
            
            // Check two categories of members for being an initialization point:
            //   1. Public fields
            //   2. Public properties
            // Check and add them to the list of initialization points if they
            // are not already registered.

            _initializationPoints = _configuredInitializationPoints ?? new List<InitializationPointSpecification>();
			
            foreach (var fieldInfo in _targetType.GetFields())
            {
                ComponentContextUtils.CheckAndAddInitializationPoint(_composer, _initializationPoints, fieldInfo);
            }

            foreach (var fieldInfo in _targetType.GetProperties())
            {
                ComponentContextUtils.CheckAndAddInitializationPoint(_composer, _initializationPoints, fieldInfo);
            }
        }

        private void LoadCompositionNotificationMethods()
        {
            if (_compositionNotifications != null)
                return;
            
            var methodsFound = ComponentContextUtils.FindCompositionNotificationMethods(_targetType).ToList();
            _compositionNotifications = _configuredCompositionNotifications?.Concat(methodsFound).ToList() ?? methodsFound;
        }
        
        private List<object> ApplyInitializationPoints(object originalComponentInstance, IComposer dependencyResolver)
        {
            var initializationPointResults = new List<object>();

            foreach (var initializationPoint in _initializationPoints)
            {
                if (initializationPoint.Query == null)
                    throw new CompositionException(
                        $"Query is null for initialization point '{initializationPoint.Name}' on component instance of type '{_targetType.FullName}'");

                if (initializationPoint.Query.IsResolvable(dependencyResolver))
                {
                    var initializationPointResult = initializationPoint.Query.Query(dependencyResolver);

                    initializationPointResults.Add(initializationPointResult);
                    ComponentContextUtils.ApplyInitializationPoint(originalComponentInstance,
                        initializationPoint.Name,
                        initializationPoint.MemberType,
                        initializationPointResult);
                }
                else
                {
                    // Check if the required initialization points get a value.
                    if (initializationPoint.Required.GetValueOrDefault(_composer.Configuration.InitializationPointsRequiredByDefault))
                        throw new CompositionException(
                            $"Could not fill initialization point '{initializationPoint.Name}' of type '{_targetType.FullName}'.");
					
                    initializationPointResults.Add(null);
                }
				
            }

            return initializationPointResults;
        }
        
        private void InvokeCompositionNotifications(object componentInstance, IComposer dependencyResolver)
        {
            if (_compositionNotifications == null)
                return;

            foreach (var method in _compositionNotifications)
            {
                method(dependencyResolver, componentInstance);
            }
        }
    }
}