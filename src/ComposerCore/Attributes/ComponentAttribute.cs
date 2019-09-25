using System;

namespace ComposerCore.Attributes
{
	/// <summary>
	/// Marks a class as being a "Component", which can then provide functionality to
	/// other components based on provided contracts, and ask for functionality from
	/// other components by having "Plugs", based on required contracts.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
	public sealed class ComponentAttribute : Attribute
	{
		public ComponentAttribute()
			: this(null)
		{
		}

		public ComponentAttribute(string defaultName)
		{
			DefaultName = defaultName;
		}

		public string DefaultName { get; }
	}
}