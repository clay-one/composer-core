using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ComposerCore.Attributes;
using ComposerCore.Extensibility;
using ComposerCore.Implementation;

namespace ComposerCore.Factories
{
    public class LocalComponentBuilder
    {
        private readonly Type _targetType;
        private IComposer _composer;
        
        private List<ConstructorArgSpecification> _configuredConstructorArgSpecs;
        private ConstructorInfo _resolvedConstructor;
        private List<ConstructorArgSpecification> _constructorArgSpecs;
        private ConstructorResolutionPolicy? _constructorResolutionPolicy;

        public LocalComponentBuilder(Type targetType, LocalComponentBuilder original = null)
        {
            _targetType = targetType ?? throw new ArgumentNullException(nameof(targetType));
            _constructorResolutionPolicy = original?._constructorResolutionPolicy;
            _configuredConstructorArgSpecs = original?._configuredConstructorArgSpecs == null
	            ? null
	            : new List<ConstructorArgSpecification>(original._configuredConstructorArgSpecs);
            
            _resolvedConstructor = null;
            _constructorArgSpecs = null;
        }

        public ConstructorResolutionPolicy? ConstructorResolutionPolicy
        {
	        get => _constructorResolutionPolicy;
	        set
	        {
		        if (_resolvedConstructor != null)
			        throw new InvalidOperationException($"The constructor is already resolved for type {_targetType.FullName} and setting the resolution policy will have no effect.");

		        _constructorResolutionPolicy = value;
	        }
        }

        public void Initialize(IComposer composer)
        {
	        _composer = composer;
        }

        public void AddConfiguredConstructorArg(ConstructorArgSpecification cas)
        {
	        if (_configuredConstructorArgSpecs == null)
		        _configuredConstructorArgSpecs = new List<ConstructorArgSpecification>();
	        
	        _configuredConstructorArgSpecs.Add(cas);
        }

        public object Build()
        {
	        if (_composer == null)
		        throw new InvalidOperationException("LocalComponentBuilder should be Initialized before calling Build");
	        
	        var configuredArguments = PrepareConstructorArguments(_configuredConstructorArgSpecs);
	        var configuredArgTypes = configuredArguments?.Select(a => a?.GetType()).ToArray();
	        var candidates = _targetType.GetConstructors().Where(c => IsCandidateConstructor(c, configuredArgTypes))
		        .ToArray();
	        
            ResolveConstructor(candidates);
            PrepareConstructorSpecs();
			
            // Prepare the constructor information for instantiating the object.

            var arguments = PrepareConstructorArguments(_constructorArgSpecs, configuredArguments);
            return _resolvedConstructor.Invoke(arguments.ToArray());
        }

        private bool IsCandidateConstructor(ConstructorInfo constructorInfo, Type[] configuredArgTypes)
        {
	        if (constructorInfo == null || !constructorInfo.IsPublic)
		        return false;

	        if (configuredArgTypes == null || configuredArgTypes.Length == 0)
		        return true;

	        var parameters = constructorInfo.GetParameters();
	        if (configuredArgTypes.Length > parameters.Length)
		        return false;

	        for (var i = 0; i < configuredArgTypes.Length; i++)
	        {
		        if (configuredArgTypes[i] != null &&
		            !parameters[i].ParameterType.IsAssignableFrom(configuredArgTypes[i]))
		        {
			        return false;
		        }
	        }

	        return true;
        }
        
        private void ResolveConstructor(ConstructorInfo[] candidateConstructors)
        {
	        if (_resolvedConstructor != null)
		        return;

	        if (candidateConstructors == null || candidateConstructors.Length == 0)
		        throw new CompositionException("There are no candidate constructors for the component of type " +
		                                       $"'{_targetType.FullName}'. Candidate constructors should be public. " +
		                                       "If any arguments are provided for the component on its registration, " +
		                                       "the parameter types should be compatible.");

	        var policy = ConstructorResolutionPolicy?.ToString();
	        if (policy == null)
	        {
		        var policyAttribute = ComponentContextUtils.GetComponentConstructorResolutionAttribute(_targetType);
		        policy = policyAttribute == null
			        ? _composer.Configuration.DefaultConstructorResolutionPolicy.ToString()
			        : policyAttribute.ConstructorResolutionPolicy;
	        }
            
	        var resolver = 
		        _composer.GetComponent<IConstructorResolver>(policy) ??
                _composer.GetComponent<IConstructorResolver>() ??
		        throw new CompositionException($"Could not find an appropriate IConstructorResolver for {policy} " +
		                                       "policy in the context, while attempting to build the component " +
		                                       $"type {_targetType.FullName}.");
	        
	        _resolvedConstructor = resolver.Resolve(_targetType, candidateConstructors, _configuredConstructorArgSpecs?.ToArray()) ?? 
	                               throw new CompositionException("Could not resolve a constructor for component " +
	                                                              $"type {_targetType.FullName} using policy " +
	                                                              $"'{policy} among the " +
	                                                              $"{candidateConstructors.Length} candidates.");
        }

        private void PrepareConstructorSpecs()
        {
	        if (_constructorArgSpecs != null)
		        return;

	        _constructorArgSpecs = ConstructorArgSpecification.BuildFrom(_resolvedConstructor);
        }

        private List<object> PrepareConstructorArguments(
	        IReadOnlyList<ConstructorArgSpecification> specs, 
	        List<object> current = null)
        {
	        if (specs == null)
		        return current;
            
            var constructorArguments = new List<object>();
            if (current != null)
	            constructorArguments.AddRange(current);

            while (constructorArguments.Count < specs.Count)
            {
	            // Pick the next spec to be added to result
	            var cas = specs[constructorArguments.Count];

	            if (cas.Query == null)
		            throw new CompositionException("Query is null for a constructor argument, for the type '" +
		                                           _targetType.FullName + "'");

	            var argumentValue = cas.Query.Query(_composer);
	            if (argumentValue == null && cas.Required.GetValueOrDefault(_composer.Configuration.ConstructorArgumentRequiredByDefault))
		            throw new CompositionException("Required constructor argument can not be queried for type " +
		                                           $"'{_targetType.FullName}' and query '{cas.Query}'");

	            constructorArguments.Add(argumentValue);
            }

            return constructorArguments;
        }
    }
}