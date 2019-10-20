using System;

namespace ComposerCore.Attributes
{
	/// <summary>
	/// Specifies a one-to-many composition point, by declaring required functionality, so that the Composer
	/// should fill the field or attribute with a collection of appropriate component instaces to fulfill the request.
	/// </summary>
	/// <remarks>
	/// Same as [ComponentPlug], but with multiplicity.
	/// The property / field should be either of the types IEnumerable, ICollection, IList with a type parameter
	/// corresponding to the type of target contract, or an array of the target contract type.
	/// </remarks>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
	[Obsolete("Use [ComponentPlug] with the IEnumerable<T> as the element type instead.")]
	public sealed class ComponentMultiPlugAttribute : Attribute
	{
		public ComponentMultiPlugAttribute()
			: this(null)
		{
		}

		public ComponentMultiPlugAttribute(string name)
		{
			Name = name;
		}

		public string Name { get; }
	}
}