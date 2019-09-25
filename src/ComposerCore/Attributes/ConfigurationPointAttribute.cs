using System;

namespace ComposerCore.Attributes
{
	/// <summary>
	/// Specifies a Configuration Point in a component, where a configuration
	/// value should be set by the composer in order for the component to work
	/// properly.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
	public class ConfigurationPointAttribute : Attribute
	{
		public ConfigurationPointAttribute()
			: this(null)
		{
		}

		public ConfigurationPointAttribute(bool required)
			: this(null, required)
		{
		}

		public ConfigurationPointAttribute(string configurationVariableName)
		{
			ConfigurationVariableName = configurationVariableName;
			Required = null;
		}

		public ConfigurationPointAttribute(string configurationVariableName, bool required)
		{
			ConfigurationVariableName = configurationVariableName;
			Required = required;
		}

		public string ConfigurationVariableName { get; }

		public bool? Required { get; }
	}
}