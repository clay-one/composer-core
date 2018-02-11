using System;

namespace ComposerCore.Attributes
{
	/// <summary>
	/// Specifies a composition point, by declaring required functionality, so that the Composer
	/// should fill the field or attribute with appropriate component instace to fulfill the request.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
	public sealed class ComponentPlugAttribute : Attribute
	{
		public ComponentPlugAttribute()
			: this(null)
		{
		}

		public ComponentPlugAttribute(bool required)
			: this(null, required)
		{
		}

		public ComponentPlugAttribute(string name, bool required = true)
		{
			Name = name;
			Required = required;
		}

		public string Name { get; }

		public bool Required { get; }
	}
}