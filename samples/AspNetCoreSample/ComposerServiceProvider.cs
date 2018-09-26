using System;
using ComposerCore;

namespace AspNetCoreSample
{
    public class ComposerServiceProvider : IServiceProvider
    {
        private readonly IComposer _composer;
        public ComposerServiceProvider(IComposer composer)
        {
            _composer = composer ?? throw new ArgumentNullException(nameof(composer));
        }

        public object GetService(Type serviceType)
        {
            return _composer.GetComponent(serviceType);
        }
    }
}