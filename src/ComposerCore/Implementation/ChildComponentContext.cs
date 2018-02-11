using System;

namespace ComposerCore.Implementation
{
    public class ChildComponentContext : ComponentContext
    {
        private ComponentContext _parentContext;

        public ChildComponentContext(ComponentContext parentContext) : base(false)
        {
            _parentContext = parentContext ?? throw new ArgumentNullException(nameof(parentContext));
        }


    }
}