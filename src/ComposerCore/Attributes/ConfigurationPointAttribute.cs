using System;

namespace ComposerCore.Attributes
{
	/// <summary>
	/// Specifies a Configuration Point in a component, where a configuration
	/// value should be set by the composer in order for the component to work
	/// properly.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, Inherited = false, AllowMultiple = false)]
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

	/// <summary>
	/// Syntactic sugar for ConfigurationPointAttribute to improve readability
	/// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, Inherited = false,
		AllowMultiple = false)]
	public class ConfigAttribute : ConfigurationPointAttribute
	{
		public ConfigAttribute()
		{
		}

		public ConfigAttribute(bool required) : base(required)
		{
		}

		public ConfigAttribute(string configurationVariableName) : base(configurationVariableName)
		{
		}

		public ConfigAttribute(string configurationVariableName, bool required) : base(configurationVariableName, required)
		{
		}
	}
}