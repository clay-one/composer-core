using System;
using ComposerCore.Factories;
using ComposerCore.Implementation;

namespace ComposerCore.FluentExtensions
{
    [Obsolete("Use ComponentContext.RegisterObject overloads instead")]
    public class FluentPreInitializedComponentConfig
    {
        private readonly ComponentContext _context;
        private readonly PreInitializedComponentFactory _factory;

        #region Constructors

        public FluentPreInitializedComponentConfig(ComponentContext context, object componentInstance)
        {
            _context = context;
            _factory = new PreInitializedComponentFactory(componentInstance);
        }

        #endregion

        #region Fluent configuration methods

        [Obsolete("Use ComponentContext.RegisterObject overloads instead")]
        public void Register(string contractName = null)
        {
            _context.Register(contractName, _factory);
        }

        [Obsolete("Use ComponentContext.RegisterObject overloads instead")]
        public void RegisterWith<TContract>(string contractName = null)
        {
            RegisterWith(typeof(TContract), contractName);
        }

        [Obsolete("Use ComponentContext.RegisterObject overloads instead")]
        public void RegisterWith(Type contractType, string contractName = null)
        {
            _context.Register(contractType, contractName, _factory);
        }

        #endregion

    }
}