using System;
using ComposerCore.Attributes;

namespace ComposerCore.AspNet
{
    [Component, Scoped]
    public class ComposerServiceProvider : IServiceProvider
    {
        private readonly IComposer _scope;

        [CompositionConstructor]
        public ComposerServiceProvider(IComposer scope)
        {
            _scope = scope;
        }

        public object GetService(Type serviceType)
        {
            return _scope.GetComponent(serviceType);
        }
    }
}