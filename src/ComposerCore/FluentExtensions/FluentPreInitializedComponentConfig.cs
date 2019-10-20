using System;
using ComposerCore.Factories;
using ComposerCore.Implementation;

namespace ComposerCore.FluentExtensions
{
    [Obsolete("Use ComponentContext.RegisterObject overloads instead")]
    public class FluentPreInitializedComponentConfig
    {
        private readonly ComponentContext _context;
        private readonly object _componentInstance;

        #region Constructors

        public FluentPreInitializedComponentConfig(ComponentContext context, object componentInstance)
        {
            _context = context;
            _componentInstance = componentInstance;
        }

        #endregion

        #region Fluent configuration methods

        [Obsolete("Use ComponentContext.RegisterObject overloads instead")]
        public void Register()
        {
            _context.RegisterObject(_componentInstance);
        }

        [Obsolete("Use ComponentContext.RegisterObject overloads instead")]
        public void Register(string contractName)
        {
            _context.RegisterObject(contractName, _componentInstance);
        }

        [Obsolete("Use ComponentContext.RegisterObject overloads instead")]
        public void RegisterWith<TContract>()
        {
            RegisterWith(typeof(TContract));
        }
        
        [Obsolete("Use ComponentContext.RegisterObject overloads instead")]
        public void RegisterWith<TContract>(string contractName)
        {
            RegisterWith(typeof(TContract), contractName);
        }

        [Obsolete("Use ComponentContext.RegisterObject overloads instead")]
        public void RegisterWith(Type contractType)
        {
            _context.RegisterObject(contractType, _componentInstance);
        }
        
        [Obsolete("Use ComponentContext.RegisterObject overloads instead")]
        public void RegisterWith(Type contractType, string contractName)
        {
            _context.RegisterObject(contractType, contractName, _componentInstance);
        }

        #endregion

    }
}