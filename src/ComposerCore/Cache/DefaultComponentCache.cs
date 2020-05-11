using System.Collections.Generic;
using ComposerCore.Attributes;
using ComposerCore.Extensibility;

namespace ComposerCore.Cache
{
	[Component(nameof(DefaultComponentCache)), Transient, ConstructorResolutionPolicy(null)]
	public class DefaultComponentCache : PerContractComponentCache
	{
		[CompositionConstructor]
		public DefaultComponentCache(IComposer composer) : base(composer)
		{
		}
	}
}