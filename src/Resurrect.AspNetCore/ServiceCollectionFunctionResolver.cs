using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Resurrect.Internals;

namespace Resurrect.AspNetCore
{
    public class ServiceCollectionFunctionResolver : IFunctionResolver
    {
        private readonly IServiceProvider _serviceProvider;

        public ServiceCollectionFunctionResolver(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public object ResolveInstance(ResurrectedFunction function)
        {
            var constructors = function.Type.GetConstructors();
            var constructorInfo =
                constructors.FirstOrDefault(c => c.GetCustomAttribute<ResurrectConstructorAttribute>() != null) ??
                constructors.OrderByDescending(c => c.GetParameters().Length).FirstOrDefault();
            
            if (constructorInfo is null)
            {
                throw new ResurrectException($"No suitable constructor found for {function.Type.Name}");
            }
            
            var scope = _serviceProvider.CreateScope();
            
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