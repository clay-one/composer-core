using System;
using System.Collections.Generic;
using System.Reflection;
using ComposerCore.Attributes;
using ComposerCore.CompositionalQueries;
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
	        
	        var resolver = 
		        _composer.GetComponent<IConstructorResolver>(nameof(ConstructorResolutionPolicy.SingleOrDefault)) ??
		        throw new CompositionException("Could not find an appropriate IConstructorResolver in the context.");
	        
	        _resolvedConstructor = resolver.Resolve(_targetType) ?? 
	                               throw new CompositionException("Could not resolve a constructor.");
        }

        private void PrepareConstructorSpecs()
        {
	        if (_constructorArgSpecs != null)
		        return;
	     
            _constructorArgSpecs = new List<ConstructorArgSpecification>();
            string[] queryNames = null;
            if (ComponentContextUtils.HasCompositionConstructorAttribute(_resolvedConstructor))
                queryNames = ComponentContextUtils.GetCompositionConstructorAttribute(_resolvedConstructor).Names;

            foreach (var parameterInfo in _resolvedConstructor.GetParameters())
            {
                if (!_composer.Configuration.DisableAttributeChecking && !ComponentContextUtils.HasContractAttribute(parameterInfo.ParameterType))
                    throw new CompositionException(
                        $"Parameter '{parameterInfo.Name}' of the constructor of type '{_targetType.FullName}' is not of a Contract type. " +
                        "All parameters of the composition constructor must be of Contract types, so that Composer can query for a component and pass it to them.");

                var contractName = queryNames != null && queryNames.Length > parameterInfo.Position ? 
                    queryNames[parameterInfo.Position] : null;
                
                _constructorArgSpecs.Add(
                    new ConstructorArgSpecification(
                        true, 
                        new ComponentQuery(parameterInfo.ParameterType, contractName)));
            }

            if (queryNames != null && queryNames.Length > _constructorArgSpecs.Count)
                throw new CompositionException($"Extra names are specified for the constructor of type '{_targetType.FullName}'");
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

                if ((argumentValue == null) && (cas.Required))
                    throw new CompositionException("Required constructor argument can not be queried for type '" +
                                                   _targetType.FullName + "'");

                constructorArguments.Add(argumentValue);
            }
            return constructorArguments;
        }
    }
}