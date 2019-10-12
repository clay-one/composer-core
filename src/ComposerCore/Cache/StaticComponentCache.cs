using System.Collections.Generic;
using ComposerCore.Attributes;
using ComposerCore.Extensibility;

namespace ComposerCore.Cache
{
	[Contract, Component, ComponentCache(null), ConstructorResolutionPolicy(null)]
	public class StaticComponentCache
	{
		[CompositionConstructor]
		public StaticComponentCache()
		{
		}
	}
}