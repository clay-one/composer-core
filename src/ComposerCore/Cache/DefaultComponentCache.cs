using System.Collections.Generic;
using ComposerCore.Attributes;
using ComposerCore.Extensibility;

namespace ComposerCore.Cache
{
	[Contract, Component, ComponentCache(null), ConstructorResolutionPolicy(null)]
	public class DefaultComponentCache
	{
		[CompositionConstructor]
		public DefaultComponentCache()
		{
		}
	}
}