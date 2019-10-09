using System;
using System.Collections.Generic;
using System.Linq;
using ComposerCore.Attributes;
using ComposerCore.Cache;
using ComposerCore.FluentExtensions;
using ComposerCore.Implementation;
using ComposerCore.Utility;
using Microsoft.AspNetCore.Http;
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
            composer
                .ForComponent<ComposerServiceProvider>()
                .AsSingleton()
                .RegisterWith<IServiceProvider>();
            
            composer
                .ForComponent<HttpContextAccessor>()
                .AsSingleton()
                .RegisterWith<IHttpContextAccessor>();
            
            composer
                .ForComponent<ComposerServiceScopeFactory>()
                .AsSingleton()
                .RegisterWith<IServiceScopeFactory>();

            composer.ForComponent<AspNetCoreRequestComponentCache>().Register();
            
            foreach (var service in serviceCollection)
            {
                Console.WriteLine($"service: {service.ServiceType.FullName}");
                if (service.ImplementationType != null)
                {
                    if (service.ImplementationType.IsOpenGenericType())
                    {
                        composer
                            .ForGenericComponent(service.ImplementationType)
                            .UseComponentCache(MapComponentCacheType(service.Lifetime))
                            .RegisterWith(service.ServiceType);
                    }
                    else
                    {
                        composer
                            .ForComponent(service.ImplementationType)
                            .UseComponentCache(MapComponentCacheType(service.Lifetime))
                            .RegisterWith(service.ServiceType);
                    }
                }
                else if (service.ImplementationFactory != null)
                {
                    composer
                        .ForUntypedDelegate((c) => service.ImplementationFactory(c.GetComponent<IServiceProvider>()))
                        .UseComponentCache(MapComponentCacheType(service.Lifetime))
                        .RegisterWith(service.ServiceType);
                }
                else
                {
                    composer.RegisterObject(service.ServiceType, service.ImplementationInstance);
                }
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
                    return typeof(AspNetCoreRequestComponentCache);
                    
                default:
                    return typeof(DefaultComponentCache);
            }
        }
    }
}