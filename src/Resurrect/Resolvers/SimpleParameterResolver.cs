using System;
using System.Collections.Generic;

namespace Resurrect.Resolvers
{
    public class SimpleParameterResolver : IParameterResolver
    {
        /// <summary>
        /// Resolves the parameters for a function using the provided parameters.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public object[] Resolve(Dictionary<Type, object> parameters)
        {
            var resolvedParameters = new List<object>();
            
            foreach (var parameter in parameters)
            {
                var type = parameter.Key;
                var value = parameter.Value;
                
                resolvedParameters.Add(Convert.ChangeType(value, type));
            }

            return resolvedParameters.ToArray();
        }
    }
}