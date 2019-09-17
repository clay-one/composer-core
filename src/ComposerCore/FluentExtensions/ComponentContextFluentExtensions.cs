using System;
using ComposerCore.Factories;
using ComposerCore.Implementation;

namespace ComposerCore.FluentExtensions
{
    public static class ComponentContextFluentExtensions
    {
        public static FluentLocalComponentConfig<T> ForComponent<T>(this ComponentContext context)
        {
            return new FluentLocalComponentConfig<T>(context);
        }

        public static FluentLocalComponentConfig ForComponent(this ComponentContext context, Type componentType)
        {
            return new FluentLocalComponentConfig(context, new LocalComponentFactory(componentType));
        }

        public static FluentGenericLocalComponentConfig ForGenericComponent(this ComponentContext context, Type componentType)
        {
            return new FluentGenericLocalComponentConfig(context, new GenericLocalComponentFactory(componentType));
        }

        public static FluentPreInitializedComponentConfig ForObject(this ComponentContext context, object componentInstance)
        {
            return new FluentPreInitializedComponentConfig(context, componentInstance);
        }
    }
}