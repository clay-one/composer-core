using System;
using Microsoft.Extensions.DependencyInjection;

namespace ComposerCore.AspNet
{
    public class ComposerServiceScope : IServiceScope
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IServiceProvider ServiceProvider { get; }
    }
}