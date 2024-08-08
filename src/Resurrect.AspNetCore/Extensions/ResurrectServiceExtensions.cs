using System;
using Microsoft.Extensions.DependencyInjection;

namespace Resurrect.AspNetCore.Extensions
{
    public static class ResurrectServiceExtensions
    {
        public static IServiceCollection AddResurrect(this IServiceCollection services)
        {
            services.AddResurrect(new ResurrectionOptions());

            return services;
        }
        
        public static IServiceCollection AddResurrect(this IServiceCollection services,
            Action<ResurrectionOptions> configure)
        {
            var options = new ResurrectionOptions();
            configure(options);

            services.AddResurrect(options);

            return services;
        }

        public static IServiceCollection AddResurrect(this IServiceCollection services, ResurrectionOptions options)
        {
            services.AddSingleton<IFunctionResolver, ServiceCollectionFunctionResolver>();
            services.AddSingleton<IParameterResolver, JsonParameterResolver>();

            services.AddSingleton(sp =>
            {
                if (options.ParameterResolver == null)
                {
                    options.ParameterResolver = sp.GetRequiredService<IParameterResolver>();
                }

                if (options.FunctionResolver == null)
                {
                    options.FunctionResolver = sp.GetRequiredService<IFunctionResolver>();
                }

                return options;
            });

            services.AddSingleton<Resurrector>();

            return services;
        }
    }
}