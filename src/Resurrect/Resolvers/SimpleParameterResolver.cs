using System;
using System.Collections.Generic;
using System.Linq;

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
            return parameters.Values.ToArray();
        }
    }
}