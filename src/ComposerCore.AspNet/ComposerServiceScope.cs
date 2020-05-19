using System;
using Microsoft.Extensions.DependencyInjection;

namespace ComposerCore.AspNet
{
    public class ComposerServiceScope : IServiceScope
    {
        private readonly IComposer _scope;
        public IServiceProvider ServiceProvider { get; }

        public ComposerServiceScope(IComposer scope)
        {
            _scope = scope;
            ServiceProvider = _scope.GetComponent<IServiceProvider>();
        }

        public void Dispose()
        {
            _scope.Dispose();
        }
    }
}