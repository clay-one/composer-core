using System;
using ComposerCore.Attributes;

namespace ComposerCore.AspNet
{
    [Component]
    public class ComposerServiceProvider : IServiceProvider
    {
        private readonly IComposer _composer;

        [CompositionConstructor]
        public ComposerServiceProvider(IComposer composer)
        {
            _composer = composer;
        }

        public object GetService(Type serviceType)
        {
            Console.WriteLine($"Querying composer for '{serviceType.FullName}'");
            return _composer.GetComponent(serviceType);
        }
    }
}