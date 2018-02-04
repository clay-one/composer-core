using System;

namespace ComposerCore.Definitions
{
	/// <summary>
	/// Specifies the constructor to be used to create the component instance when composing.
	/// Should be used only once per component (no more than one constructor in each class
	/// should have this attribute).
	/// </summary>
	[AttributeUsage(AttributeTargets.Constructor, Inherited = false, AllowMultiple = false)]
	public sealed class CompositionConstructorAttribute : Attribute
	{
		public CompositionConstructorAttribute()
		{
		}

		public CompositionConstructorAttribute(params string[] names)
		{
			Names = names;
		}

		public string[] Names { get; }
	}
}