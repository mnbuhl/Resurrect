using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Resurrect.Internals
{
    internal class FunctionResolver
    {
        private readonly ResurrectionOptions _options;
        
        public FunctionResolver(ResurrectionOptions options)
        {
            _options = options;
        }
        
        internal object ResolveInstance(ResurrectedFunction function)
        {
            var constructors = function.Type.GetConstructors();
            var constructorInfo =
                constructors.FirstOrDefault(c => c.GetCustomAttribute<ResurrectConstructorAttribute>() != null) ??
                constructors.OrderByDescending(c => c.GetParameters().Length).FirstOrDefault();
            
            if (constructorInfo == null)
            {
                throw new ResurrectException($"No suitable constructor found for {function.Type.Name}");
            }
            
            var scope = _options.ServiceProvider.CreateScope();

            var parameters = constructorInfo.GetParameters();
            
            var resolvedParameters = parameters.Select(p => function.Parameters.TryGetValue(p.Name, out var expression) 
                ? expression 
                : scope.ServiceProvider.GetRequiredService(p.ParameterType)).ToArray();
            
            return constructorInfo.Invoke(resolvedParameters);
        }
    }
}