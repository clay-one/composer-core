using System;
using ComposerCore.CompositionXml.Info;

namespace ComposerCore.Attributes
{
	/// <summary>
	/// Specifies a composition point, by declaring required functionality, so that the Composer
	/// should fill the field or attribute with appropriate component instance to fulfill the request.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, Inherited = false, AllowMultiple = false)]
	public class ComponentPlugAttribute : Attribute
	{
		public ComponentPlugAttribute()
			: this(null)
		{
		}

		public ComponentPlugAttribute(bool required)
			: this(null, required)
		{
		}

		public ComponentPlugAttribute(string name)
		{
			Name = name;
			Required = null;
		}
		
		public ComponentPlugAttribute(string name, bool required)
		{
			Name = name;
			Required = required;
		}

		public string Name { get; }

		public bool? Required { get; }
	}

	/// <summary>
	/// Syntactic sugar for ComponentPlugAttribute to improve readability
	/// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, Inherited = false, AllowMultiple = false)]
	public class PlugAttribute : ComponentPlugAttribute
	{
		public PlugAttribute()
		{
		}

		public PlugAttribute(bool required) : base(required)
		{
		}

		public PlugAttribute(string name) : base(name)
		{
		}

		public PlugAttribute(string name, bool required) : base(name, required)
		{
		}
	}
}