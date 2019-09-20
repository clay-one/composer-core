using System;
using ComposerCore.Factories;
using ComposerCore.Implementation;

namespace ComposerCore.FluentExtensions
{
    public static class ComponentContextFluentExtensions
    {
        public static FluentUntypedDelegateComponentConfig ForUntypedDelegate(this ComponentContext context, Func<IComposer, object> func)
        {
            return new FluentUntypedDelegateComponentConfig(context, func);
        }
    }
}