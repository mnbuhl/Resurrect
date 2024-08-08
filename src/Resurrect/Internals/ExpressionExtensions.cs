using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Resurrect.Internals
{
    internal static class ExpressionExtensions
    {
        internal static Function ToFunction<T>(this Expression<Action<T>> expression)
        {
            var methodCallExpression = expression.Body as MethodCallExpression 
                                       ?? throw new InvalidOperationException("Expression is not a method call.");
            
            return ToFunction(methodCallExpression);
        }
        
        internal static Function ToFunction<T>(this Expression<Func<T, Task>> expression)
        {
            var methodCallExpression = expression.Body as MethodCallExpression 
                                       ?? throw new InvalidOperationException("Expression is not a method call.");
            
            return ToFunction(methodCallExpression);
        }
        
        private static Function ToFunction(MethodCallExpression expression)
        {
            var method = expression.Method;
            var type = method.DeclaringType 
                       ?? throw new InvalidOperationException("Method does not have a declaring type.");

            var arguments = new List<object>();
            
            foreach (var argument in expression.Arguments)
            {
                switch (argument.NodeType)
                {
                    case ExpressionType.Constant:
                        arguments.Add(((ConstantExpression)argument).Value);
                        break;
                    case ExpressionType.New:
                    case ExpressionType.MemberAccess:
                        arguments.Add(Expression.Lambda(argument).Compile().DynamicInvoke());
                        break;
                    default:
                        throw new InvalidOperationException("Unsupported argument type.");
                }
            }
            
            var function = new Function
            {
                Type = type.AssemblyQualifiedName,
                Method = method.Name,
                Parameters = arguments.ToDictionary(a => a.GetType().AssemblyQualifiedName, a => a)
            };
            
            return function;
        }
    }
}