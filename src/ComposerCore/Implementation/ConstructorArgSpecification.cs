using System.Collections.Generic;
using System.Reflection;
using ComposerCore.CompositionalQueries;
using ComposerCore.Extensibility;

namespace ComposerCore.Implementation
{
	public class ConstructorArgSpecification
	{
		public ConstructorArgSpecification(bool? required, ICompositionalQuery query = null)
		{
			Required = required;
			Query = query;
		}

		public bool? Required { get; }
		public ICompositionalQuery Query { get; set; }

		public bool IsResolvable(IComposer composer)
		{
			return !Required.GetValueOrDefault(composer.Configuration.ConstructorArgumentRequiredByDefault) ||
			       Query.IsResolvable(composer);
		}

		public static List<ConstructorArgSpecification> BuildFrom(ConstructorInfo constructorInfo)
		{
			var result = new List<ConstructorArgSpecification>();

			string[] queryNames = null;
			if (ComponentContextUtils.HasCompositionConstructorAttribute(constructorInfo))
				queryNames = ComponentContextUtils.GetCompositionConstructorAttribute(constructorInfo).Names;

			foreach (var parameterInfo in constructorInfo.GetParameters())
			{
				// Check this condition to provide backward-compatibility with specifying names on the
				// [CompositionConstructor] attribute:
				// If the names are specified on the attribute, it will be used. Otherwise, the attribute
				// on the argument itself will be taken into account.
				
				if (queryNames == null || queryNames.Length == 0)
				{
					result.Add(BuildFrom(parameterInfo));
				}
				else
				{
					var contractName = queryNames.Length > parameterInfo.Position ? queryNames[parameterInfo.Position] : null;
					result.Add(new ConstructorArgSpecification(true, new ComponentQuery(parameterInfo.ParameterType, contractName)));
				}
			}

			if (queryNames != null && queryNames.Length > result.Count)
				throw new CompositionException($"Extra names are specified for the constructor of type '{constructorInfo.ReflectedType?.FullName ?? ""}'");
			
			return result;
		}

		public static ConstructorArgSpecification BuildFrom(ParameterInfo parameterInfo)
		{
			var attribute = ComponentContextUtils.GetComponentPlugAttribute(parameterInfo);
			
			return new ConstructorArgSpecification(
				attribute?.Required, 
				new ComponentQuery(parameterInfo.ParameterType, attribute?.Name));
		}
	}
}
