using System;
using ComposerCore.Definitions.Cache;

namespace ComposerCore.Definitions
{
	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
	public class ComponentCacheAttribute : Attribute
	{
		public ComponentCacheAttribute()
			: this(typeof(DefaultComponentCache))
		{
		}

		public ComponentCacheAttribute(Type componentCacheType, string componentCacheName = null)
		{
			ComponentCacheType = componentCacheType;
			ComponentCacheName = componentCacheName;
		}

		public Type ComponentCacheType { get; }

		public string ComponentCacheName { get; }
	}
}