using System;
using System.Collections.Generic;
using ComposerCore.Attributes;
using ComposerCore.Cache;
using ComposerCore.FluentExtensions;
using ComposerCore.Implementation;
using ComposerCore.Utility;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;

namespace ComposerCore.AspNet
{
    public static class ComposerAspNetCoreUtils
    {
        public static ComponentContext BuildContext(IEnumerable<ServiceDescriptor> serviceCollection)
        {
            var composer = new ComponentContext();
            composer.Configuration.DisableAttributeChecking = true;
            composer.Configuration.DefaultConstructorResolutionPolicy = ConstructorResolutionPolicy.MostResolvable;
            composer.Populate(serviceCollection);

            return composer;
        }
        
        public static void Populate(this ComponentContext composer, IEnumerable<ServiceDescriptor> serviceCollection)
        {
            RegisterAspNetScopeComponents(composer);

            foreach (var service in serviceCollection)
            {
                RegisterAspNetService(composer, service);
            }

            ReplaceDefaultAspNetComponents(composer);
        }

        private static void RegisterAspNetScopeComponents(ComponentContext composer)
        {
            composer
                .ForComponent<ComposerServiceProvider>()
                .AsScoped()
                .RegisterWith<IServiceProvider>();

            composer
                .ForComponent<ComposerServiceScopeFactory>()
                .AsScoped()
                .RegisterWith<IServiceScopeFactory>();
        }

        private static void RegisterAspNetService(ComponentContext composer, ServiceDescriptor service)
        {
            var componentCacheType = MapComponentCacheType(service.Lifetime);
            Console.WriteLine($"service: {service.ServiceType.FullName} - Cache type: {componentCacheType?.Name ?? "<NULL>"}");
            if (service.ImplementationType != null)
            {
                if (service.ImplementationType.IsOpenGenericType())
                {
                    composer
                        .ForGenericComponent(service.ImplementationType)
                        .UseComponentCache(componentCacheType)
                        .RegisterWith(service.ServiceType);
                }
                else
                {
                    composer
                        .ForComponent(service.ImplementationType)
                        .UseComponentCache(componentCacheType)
                        .RegisterWith(service.ServiceType);
                }
            }
            else if (service.ImplementationFactory != null)
            {
                composer
                    .ForUntypedFactoryMethod((c) => service.ImplementationFactory(c.GetComponent<IServiceProvider>()))
                    .UseComponentCache(componentCacheType)
                    .RegisterWith(service.ServiceType);
            }
            else
            {
                composer.RegisterObject(service.ServiceType, service.ImplementationInstance);
            }
        }

        private static Type MapComponentCacheType(ServiceLifetime lifetime)
        {
            switch (lifetime)
            {
                case ServiceLifetime.Singleton:
                    return typeof(DefaultComponentCache);
                
                case ServiceLifetime.Transient:
                    return null;
                
                case ServiceLifetime.Scoped:
                    return typeof(ScopedComponentCache);
                    
                default:
                    return typeof(DefaultComponentCache);
            }
        }
        
        private static void ReplaceDefaultAspNetComponents(ComponentContext composer)
        {
            var controllerFactory = composer.GetComponent<IControllerFactory>();
            composer.UnregisterFamily(typeof(IControllerFactory));
            composer.RegisterObject<IControllerFactory>(new ComposerControllerFactory(controllerFactory, composer));
        }
    }
}