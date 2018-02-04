using System;

namespace ComposerCore.Definitions
{
	/// <summary>
	/// Designates a field or property to be a Resource Manager Plug, in order for the
	/// component to recieve ResourceManager objects similar to the components.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
	public class ResourceManagerPlugAttribute : Attribute
	{
		public ResourceManagerPlugAttribute(string id, bool required = true)
		{
			Id = id;
			Required = required;
		}

		public string Id { get; }

		public bool Required { get; }
	}
}