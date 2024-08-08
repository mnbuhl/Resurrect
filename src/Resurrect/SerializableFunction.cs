using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Resurrect.Internals;

namespace Resurrect
{
    [Serializable]
    public class SerializableFunction
    {
        public string Type { get; set; } = string.Empty;
        public string Method { get; set; } = string.Empty;
        public Dictionary<string, object> Parameters { get; set; } = new Dictionary<string, object>();
        
        public static SerializableFunction Serialize<T>(Expression<Action<T>> expression)
        {
            return SerializableFunctionHelper.FromExpression(expression);
        }
        
        public static SerializableFunction Serialize<T>(Expression<Func<T, Task>> expression)
        {
            return SerializableFunctionHelper.FromExpression(expression);
        }
    }
}