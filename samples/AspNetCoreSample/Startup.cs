using System;
using ComposerCore.Cache;
using ComposerCore.FluentExtensions;
using ComposerCore.Implementation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCoreSample
{
    public class Startup
    {
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            var componentContext = new ComponentContext();
            componentContext.Configuration.DisableAttributeChecking = true;
            
            foreach (var service in services)
            {
                if (service.ImplementationInstance != null)
                {
                    componentContext.ForObject(service.ImplementationInstance)
                        .RegisterWith(service.ServiceType);
                }
                else if (service.ImplementationType != null)
                {
                    componentContext.ForComponent(service.ImplementationType)
                        .UseComponentCache(GetComponentCacheType(service.Lifetime))
                        .RegisterWith(service.ServiceType);
                }
                else if (service.ImplementationFactory != null)
                {
                    // TODO
                }
            }
                        
            return new ComposerServiceProvider(componentContext);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            app.Run(async (context) => { await context.Response.WriteAsync("Hello World!"); });
        }

        private Type GetComponentCacheType(ServiceLifetime lifetime)
        {
            switch (lifetime)
            {
                case ServiceLifetime.Singleton:
                    return typeof(ContractAgnosticComponentCache);
                
                case ServiceLifetime.Scoped:
                    return null; // TODO
                
                case ServiceLifetime.Transient:
                    return null;
                
                default:
                    throw new InvalidOperationException($"Unsupported ServiceLifetime value {lifetime}");
            }
        }
    }
}