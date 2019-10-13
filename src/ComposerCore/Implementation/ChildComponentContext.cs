using System;

namespace ComposerCore.Implementation
{
    public class ChildComponentContext : ComponentContext
    {
        private ComponentContext _parentContext;

        public ChildComponentContext(ComponentContext parentContext)
        {
            _parentContext = parentContext ?? throw new ArgumentNullException(nameof(parentContext));
        }


    }
}