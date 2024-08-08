using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Resurrect.Internals;

namespace Resurrect
{
    public class Resurrector
    {
        private readonly ResurrectionOptions _options;
        
        public Resurrector(ResurrectionOptions options)
        {
            _options = options;
        }
        
        public static SerializableFunction Serialize<T>(Expression<Action<T>> expression)
        {
            return expression.ToFunction();
        }
        
        public static SerializableFunction Serialize<T>(Expression<Func<T, Task>> expression)
        {
            return expression.ToFunction();
        }

        public void Invoke(SerializableFunction serializableFunction)
        {
            var resurrectedFunction = Resurrect(serializableFunction);
            var instance = ResolveInstance(resurrectedFunction);
            
            resurrectedFunction.Method.Invoke(instance, resurrectedFunction.Parameters);
        }
        
        public T Invoke<T>(SerializableFunction serializableFunction)
        {
            var resurrectedFunction = Resurrect(serializableFunction);
            var instance = ResolveInstance(resurrectedFunction);
            
            return (T)resurrectedFunction.Method.Invoke(instance, resurrectedFunction.Parameters);
        }

        public async Task InvokeAsync(SerializableFunction serializableFunction)
        {
            var resurrectedFunction = Resurrect(serializableFunction);
            var instance = ResolveInstance(resurrectedFunction);
            
            await (Task)resurrectedFunction.Method.Invoke(instance, resurrectedFunction.Parameters);
        }
        
        public async Task<T> InvokeAsync<T>(SerializableFunction serializableFunction)
        {
            var resurrectedFunction = Resurrect(serializableFunction);
            var instance = ResolveInstance(resurrectedFunction);
            
            return await (Task<T>)resurrectedFunction.Method.Invoke(instance, resurrectedFunction.Parameters);
        }

        private object ResolveInstance(ResurrectedFunction function)
        {
            var resolver = _options.FunctionResolver;
            return resolver.ResolveInstance(function.Type);
        }
        
        private ResurrectedFunction Resurrect(SerializableFunction serializableFunction)
        {
            var type = Type.GetType(serializableFunction.Type) 
                       ?? throw new InvalidOperationException("Type could not be found.");
            
            var parameters = serializableFunction.Parameters.ToDictionary(p => Type.GetType(p.Key), p => p.Value);
            
            var method = type.GetMethod(
                             serializableFunction.Method, 
                             BindingFlags.Public|BindingFlags.Instance, null, 
                             parameters.Keys.ToArray(), 
                             null) 
                         ?? throw new InvalidOperationException("Method could not be found.");
            
            return new ResurrectedFunction
            {
                Type = type,
                Method = method,
                Parameters = _options.ParameterResolver.Resolve(parameters)
            };
        }
    }
}