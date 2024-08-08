using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Resurrect.Internals;

namespace Resurrect.AspNetCore
{
    /// <summary>
    /// Resolves instances of function types using the provided <see cref="IServiceProvider"/>
    /// </summary>
    public class ServiceCollectionFunctionResolver : IFunctionResolver
    {
        private readonly IServiceProvider _serviceProvider;

        public ServiceCollectionFunctionResolver(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Resolves an instance of a function type based on the provided type from DI container or by invoking the constructor with the most parameters
        /// </summary>
        /// <param name="functionType">Type of the function to resolve</param>
        /// <returns>Instance of the function type</returns>
        /// <exception cref="ResurrectException">Thrown when no suitable constructor is found for the function type</exception>
        public object ResolveInstance(Type functionType)
        {
            var scope = _serviceProvider.CreateScope();
            
            // Attempt to get resolve the type from the DI container
            var resolvedInstance = scope.ServiceProvider.GetService(functionType);
            
            if (resolvedInstance != null)
            {
                return resolvedInstance;
            }
            
            // If the type is not registered in the DI container, attempt to resolve it using the constructor with the most parameters
            var constructors = functionType.GetConstructors();
            var resurrectConstructor = constructors.FirstOrDefault(c => c.GetCustomAttribute<ResurrectConstructorAttribute>() != null);
            var constructorInfo = resurrectConstructor ?? constructors.OrderByDescending(c => c.GetParameters().Length).FirstOrDefault();
            
            if (constructorInfo is null)
            {
                throw new ResurrectException($"No suitable constructor found for {functionType.Name}");
            }
            
            var parameters = constructorInfo.GetParameters();
            
            var resolvedParameters = parameters.Select(p =>
            {
                if (p.ParameterType == typeof(IServiceProvider))
                {
                    return scope.ServiceProvider;
                }
                
                return scope.ServiceProvider.GetService(p.ParameterType);
            }).ToArray();
            
            return constructorInfo.Invoke(resolvedParameters);
        }
    }
}