using ComposerCore.Attributes;

namespace ComposerCore.Cache
{
	[Component(nameof(DefaultComponentCache)), Transient, ConstructorResolutionPolicy(null)]
	public class DefaultComponentCache : PerContractComponentCache
	{
		[CompositionConstructor]
		public DefaultComponentCache()
		{
		}
	}
}