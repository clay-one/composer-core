using ComposerCore.Attributes;

namespace ComposerCore.Cache
{
	[Component(nameof(TransientComponentCache)), Transient, ConstructorResolutionPolicy(null)]
    public class TransientComponentCache : PerRegistrationComponentCache
    {
	    [CompositionConstructor]
	    public TransientComponentCache(IComposer composer) : base(composer)
	    {
	    }
    }
}