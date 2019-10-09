using Microsoft.Extensions.DependencyInjection;

namespace ComposerCore.AspNet
{
    public class ComposerServiceScopeFactory : IServiceScopeFactory
    {
        public IServiceScope CreateScope()
        {
            return new ComposerServiceScope();
        }
    }
}