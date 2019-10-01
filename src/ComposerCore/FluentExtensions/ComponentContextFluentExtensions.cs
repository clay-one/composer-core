using System;
using ComposerCore.Factories;
using ComposerCore.Implementation;
using ComposerCore.Utility;

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
            if (componentType.IsOpenGenericType())
                throw new ArgumentException($"Type {componentType.FullName} is generic. Use 'ForGenericComponent' method instead.");
            
            return new FluentLocalComponentConfig(context, new LocalComponentFactory(componentType));
        }

        public static FluentGenericLocalComponentConfig ForGenericComponent(this ComponentContext context, Type componentType)
        {
            if (!componentType.IsOpenGenericType())
                throw new ArgumentException($"Type {componentType.FullName} is not generic.");
            
            return new FluentGenericLocalComponentConfig(context, new GenericLocalComponentFactory(componentType));
        }

        [Obsolete("Use ComponentContext.RegisterObject overloads instead")]
        public static FluentPreInitializedComponentConfig ForObject(this ComponentContext context, object componentInstance)
        {
            return new FluentPreInitializedComponentConfig(context, componentInstance);
        }
    }
}