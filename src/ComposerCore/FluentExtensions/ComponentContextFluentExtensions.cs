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
            
            return new FluentGenericLocalComponentConfig(context, new GenericComponentRegistration(componentType));
        }
        
        public static FluentUntypedFactoryMethodComponentConfig ForUntypedFactoryMethod(this ComponentContext context, Func<IComposer, object> func)
        {
            return new FluentUntypedFactoryMethodComponentConfig(context, func);
        }        

        public static FluentFactoryMethodComponentConfig<TComponent> ForFactoryMethod<TComponent> 
            (this ComponentContext context, Func<IComposer, TComponent> func) where TComponent : class
        {
            return new FluentFactoryMethodComponentConfig<TComponent>(context, func);
        }

        [Obsolete("Use ComponentContext.RegisterObject overloads instead")]
        public static FluentPreInitializedComponentConfig ForObject(this ComponentContext context, object componentInstance)
        {
            return new FluentPreInitializedComponentConfig(context, componentInstance);
        }
    }
}