using System;
using System.Collections.Generic;
using System.Linq;
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
            composer.Populate(serviceCollection);

            return composer;
        }
        
        public static void Populate(this ComponentContext composer, IEnumerable<ServiceDescriptor> serviceCollection)
        {
            composer
                .ForComponent<ComposerServiceProvider>()
                .UseComponentCache(typeof(DefaultComponentCache))
                .RegisterWith<IServiceProvider>();
            
            composer
                .ForComponent<HttpContextAccessor>()
                .UseComponentCache(typeof(DefaultComponentCache))
                .RegisterWith<IHttpContextAccessor>();

            composer.ForComponent<AspNetCoreRequestComponentCache>().Register();
            
            foreach (var service in serviceCollection)
            {
                Console.WriteLine($"service: {service.ServiceType.FullName}");
                if (service.ImplementationType != null)
                {
                    if (service.ImplementationType.IsOpenGenericType())
                    {
                        var constructors = service.ImplementationType.GetConstructors();
                        Type[] constructorParamTypes = null;
                    
                        foreach (var constructor in constructors)
                        {
                            if (constructorParamTypes == null || 
                                constructor.GetParameters().Length < constructorParamTypes.Length)
                            {
                                constructorParamTypes = constructor.GetParameters().Select(p => p.ParameterType).ToArray();
                            }
                        }
                    
                        var config = composer
                            .ForGenericComponent(service.ImplementationType)
                            .UseComponentCache(MapComponentCacheType(service.Lifetime));

                        foreach (var constructorParamType in constructorParamTypes ?? Enumerable.Empty<Type>())
                        {
//                            var enumerableTypeArg = constructorParamType.GetEnumerableTypeArgument();
//                            if (enumerableTypeArg != null)
//                                config.AddConstructorValue(c => c.GetAllComponents(enumerableTypeArg), false);
//                            else
                                config.AddConstructorComponent(constructorParamType, required: false);
                        }
                        
                        config.RegisterWith(service.ServiceType);
                    }
                    else
                    {
                        var constructors = service.ImplementationType.GetConstructors();
                        Type[] constructorParamTypes = null;
                    
                        foreach (var constructor in constructors)
                        {
                            if (constructorParamTypes == null || 
                                constructor.GetParameters().Length < constructorParamTypes.Length)
                            {
                                constructorParamTypes = constructor.GetParameters().Select(p => p.ParameterType).ToArray();
                            }
                        }
                    
                        var config = composer
                            .ForComponent(service.ImplementationType)
                            .UseComponentCache(MapComponentCacheType(service.Lifetime));
                            
                        foreach (var constructorParamType in constructorParamTypes ?? Enumerable.Empty<Type>())
                        {
//                            var enumerableTypeArg = constructorParamType.GetEnumerableTypeArgument();
//                            if (enumerableTypeArg != null)
//                                config.AddConstructorValue(c => c.GetAllComponents(enumerableTypeArg), false);
//                            else
                                config.AddConstructorComponent(constructorParamType, required: false);
                        }

                        config.UseConstructor(constructorParamTypes);
                        config.RegisterWith(service.ServiceType);
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
                    composer
                        .ForObject(service.ImplementationInstance)
                        .RegisterWith(service.ServiceType);
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