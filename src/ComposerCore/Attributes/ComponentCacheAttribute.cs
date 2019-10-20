using System;
using ComposerCore.Cache;

namespace ComposerCore.Attributes
{
	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
	public class ComponentCacheAttribute : Attribute
	{
		public ComponentCacheAttribute()
			: this(typeof(DefaultComponentCache))
		{
		}

		public ComponentCacheAttribute(Type componentCacheType)
		{
			ComponentCacheName = componentCacheType?.Name ?? nameof(NoComponentCache);
		}

		public string ComponentCacheName { get; set;  }
	}
}