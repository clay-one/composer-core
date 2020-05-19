using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace ComposerCore.AspNet
{
    public class ComposerControllerFactory : IControllerFactory
    {
        private readonly IControllerFactory _wrappedFactory;
        private readonly IComposer _composer;

        public ComposerControllerFactory(IControllerFactory wrappedFactory, IComposer composer)
        {
            _wrappedFactory = wrappedFactory;
            _composer = composer;
        }

        public object CreateController(ControllerContext context)
        {
            var result = _wrappedFactory.CreateController(context);
            _composer.InitializePlugs(result, result.GetType());
            return result;
        }

        public void ReleaseController(ControllerContext context, object controller)
        {
            _wrappedFactory.ReleaseController(context, controller);
        }
    }
}