using System;
using System.Linq;
using System.Linq.Expressions;
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
        
        public Function ToFunction<T>(Expression<Action<T>> expression)
        {
            return expression.ToFunction();
        }
        
        public Function ToFunction<T>(Expression<Func<T, Task>> expression)
        {
            return expression.ToFunction();
        }

        public void Invoke(Function function)
        {
            var resurrectedFunction = Resurrect(function);
            var instance = ResolveInstance(resurrectedFunction);
            
            resurrectedFunction.Method.Invoke(instance, function.Parameters.Values.ToArray());
        }
        
        public T Invoke<T>(Function function)
        {
            var resurrectedFunction = Resurrect(function);
            var instance = ResolveInstance(resurrectedFunction);
            
            return (T)resurrectedFunction.Method.Invoke(instance, function.Parameters.Values.ToArray());
        }

        public async Task InvokeAsync(Function function)
        {
            var resurrectedFunction = Resurrect(function);
            var instance = ResolveInstance(resurrectedFunction);
            
            await (Task)resurrectedFunction.Method.Invoke(instance, function.Parameters.Values.ToArray());
        }
        
        public async Task<T> InvokeAsync<T>(Function function)
        {
            var resurrectedFunction = Resurrect(function);
            var instance = ResolveInstance(resurrectedFunction);
            
            return await (Task<T>)resurrectedFunction.Method.Invoke(instance, function.Parameters.Values.ToArray());
        }

        private object ResolveInstance(ResurrectedFunction function)
        {
            var resolver = new FunctionResolver(_options);
            return resolver.ResolveInstance(function);
        }
        
        private static ResurrectedFunction Resurrect(Function function)
        {
            var type = Type.GetType(function.Type) 
                       ?? throw new InvalidOperationException("Type could not be found.");
            var method = type.GetMethod(function.Method) 
                         ?? throw new InvalidOperationException("Method could not be found.");
            
            return new ResurrectedFunction
            {
                Type = type,
                Method = method,
                Parameters = function.Parameters,
            };
        }
    }
}