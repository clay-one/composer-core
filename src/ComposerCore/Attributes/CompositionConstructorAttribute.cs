using System;

namespace ComposerCore.Attributes
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

		[Obsolete("Specifying contract names on the [CompositionConstructor] is no longer supported. Use [ComponentPlug] or [ConfigurationPoint] on the parameters instead.")]
		public CompositionConstructorAttribute(params string[] names)
		{
			Names = names;
		}

		public string[] Names { get; }
	}
}