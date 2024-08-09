using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Resurrect.Internals
{
    internal static class SerializableFunctionHelper
    {
        internal static SerializableFunction FromExpression<T>(Expression<Action<T>> expression)
        {
            var methodCallExpression = expression.Body as MethodCallExpression
                                       ?? throw new InvalidOperationException("Expression is not a method call.");

            return FromExpression(methodCallExpression);
        }

        internal static SerializableFunction FromExpression<T>(Expression<Func<T, Task>> expression)
        {
            var methodCallExpression = expression.Body as MethodCallExpression
                                       ?? throw new InvalidOperationException("Expression is not a method call.");

            return FromExpression(methodCallExpression);
        }

        private static void Validate(MethodInfo method)
        {
            if (method == null) throw new ArgumentNullException(nameof(method));

            var type = method.DeclaringType;
            var parameters = method.GetParameters();

            if (!method.IsPublic) throw new ResurrectException("Only public methods can be resurrected");
            if (type == null) throw new NotSupportedException("Functions must be tied to a type to be resurrected");

            foreach (var parameter in parameters)
            {
                if (parameter.IsOut || parameter.ParameterType.IsByRef)
                    throw new NotSupportedException("Parameters with out and ref are not supported");
            }
        }

        private static SerializableFunction FromExpression(MethodCallExpression expression)
        {
            var method = expression.Method;
            var type = method.DeclaringType ??
                       throw new NotSupportedException("Functions must be tied to a type to be resurrected");

            Validate(method);

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

            var returnType = method.ReturnType;
            var isAsync = returnType == typeof(Task) || (returnType.IsGenericType &&
                                                         method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>));

            var function = new SerializableFunction
            {
                Type = type.AssemblyQualifiedName,
                Method = method.Name,
                Parameters = arguments.ToDictionary(a => a.GetType().AssemblyQualifiedName, a => a),
                ReturnType = returnType.AssemblyQualifiedName,
                Async = isAsync
            };

            return function;
        }
    }
}