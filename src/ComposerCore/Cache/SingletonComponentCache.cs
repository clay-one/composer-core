using ComposerCore.Attributes;

namespace ComposerCore.Cache
{
    [Component(nameof(SingletonComponentCache)), Transient, ConstructorResolutionPolicy(null)]
    public class SingletonComponentCache : PerRegistrationComponentCache
    {
        [CompositionConstructor]
        public SingletonComponentCache(IComposer composer) : base(composer)
        {
        }
    }
}