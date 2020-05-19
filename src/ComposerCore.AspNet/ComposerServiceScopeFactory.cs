using ComposerCore.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace ComposerCore.AspNet
{
    [Component, Scoped]
    public class ComposerServiceScopeFactory : IServiceScopeFactory
    {
        private readonly IComposer _composer;

        [CompositionConstructor]
        public ComposerServiceScopeFactory(IComposer composer)
        {
            _composer = composer;
        }

        public IServiceScope CreateScope()
        {
            var newScope = _composer.CreateScope();
            return new ComposerServiceScope(newScope);
        }
    }
}