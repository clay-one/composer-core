using System;
using System.Collections.Generic;
using System.Reflection;
using ComposerCore.Implementation;
using ComposerCore.Resources;


namespace ComposerCore.CompositionXml
{
	internal class TypeCache
	{
		private readonly Dictionary<string, Type> _cachedTypeNames;

		public TypeCache()
		{
			_cachedTypeNames = new Dictionary<string, Type>();
			NamespaceUsings = new List<string>();

			// Import pre-defined assemblies:

			CacheAssembly(typeof(AssemblyPointer).Assembly);

			// Add pre-defined namespaces:

			NamespaceUsings.Add(typeof(IComposer).Namespace);			// ComposerCore
			NamespaceUsings.Add(typeof(IResourcePointer).Namespace);	// ComposerCore.Resources
		}

		public List<string> NamespaceUsings { get; }

		public void CacheAssembly(Assembly assembly)
		{
			var types = assembly.GetExportedTypes();

			foreach (var type in types)
			{
				_cachedTypeNames[type.FullName] = type;
			}
		}

		public Type LookupType(string typeName)
		{
			if (_cachedTypeNames.ContainsKey(typeName))
				return _cachedTypeNames[typeName];

			foreach (var namespaceUsing in NamespaceUsings)
			{
				var fullTypeName = namespaceUsing + "." + typeName;

				if (_cachedTypeNames.ContainsKey(fullTypeName))
					return _cachedTypeNames[fullTypeName];
			}

			return null;
		}
	}
}