using System;
using System.Collections.Generic;
using System.Reflection;
using ComposerCore.Extensibility;
using ComposerCore.Implementation;

namespace ComposerCore.Factories
{
    internal class LocalComponentBuilder
    {
        private readonly Type _targetType;
        private IComposer _composer;
        private ConstructorInfo _resolvedConstructor;
        private List<ConstructorArgSpecification> _constructorArgSpecs;

        public LocalComponentBuilder(Type targetType)
        {
            _targetType = targetType ?? throw new ArgumentNullException(nameof(targetType));
            _resolvedConstructor = null;
            _constructorArgSpecs = null;
        }

        public void Initialize(IComposer composer)
        {
	        _composer = composer;
        }
        
        public object Build()
        {
	        if (_composer == null)
		        throw new InvalidOperationException("LocalComponentBuilder should be Initialized before calling Build");
	        
            ResolveConstructor();
            PrepareConstructorSpecs();
			
            // Prepare the constructor information for instantiating the object.

            var constructorArguments = PrepareConstructorArguments();
            return _resolvedConstructor.Invoke(constructorArguments.ToArray());
        }

        private void ResolveConstructor()
        {
	        if (_resolvedConstructor != null)
		        return;

            var policyAttribute = ComponentContextUtils.GetComponentConstructorResolutionAttribute(_targetType);
            var policy = policyAttribute == null
                ? _composer.Configuration.DefaultConstructorResolutionPolicy.ToString()
                : policyAttribute.ConstructorResolutionPolicy;
            
	        var resolver = 
		        _composer.GetComponent<IConstructorResolver>(policy) ??
                _composer.GetComponent<IConstructorResolver>() ??
		        throw new CompositionException("Could not find an appropriate IConstructorResolver in the context.");
	        
	        _resolvedConstructor = resolver.Resolve(_targetType) ?? 
	                               throw new CompositionException("Could not resolve a constructor.");
        }

        private void PrepareConstructorSpecs()
        {
	        if (_constructorArgSpecs != null)
		        return;

	        _constructorArgSpecs = ConstructorArgSpecification.BuildFrom(_resolvedConstructor);
        }

        private List<object> PrepareConstructorArguments()
        {
            var constructorArguments = new List<object>();
            foreach (var cas in _constructorArgSpecs)
            {
                if (cas.Query == null)
                    throw new CompositionException("Query is null for a constructor argument, for the type '" +
                                                   _targetType.FullName + "'");

                object argumentValue = cas.Query.Query(_composer);

                if (argumentValue == null && cas.Required.GetValueOrDefault(_composer.Configuration.ConstructorArgumentRequiredByDefault))
                    throw new CompositionException("Required constructor argument can not be queried for type '" +
                                                   _targetType.FullName + "'");

                constructorArguments.Add(argumentValue);
            }
            return constructorArguments;
        }
    }
}