using System;

namespace ComposerCore
{
    public class ChildComponentContext : ComponentContext
    {
        private ComponentContext _parentContext;

        public ChildComponentContext(ComponentContext parentContext) : base(false)
        {
            _parentContext = parentContext ?? throw new ArgumentNullException("parentContext");
        }


    }
}