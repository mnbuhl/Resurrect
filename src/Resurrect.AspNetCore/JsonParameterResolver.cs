using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Resurrect.AspNetCore
{
    public class JsonParameterResolver : IParameterResolver
    {
        /// <summary>
        /// Resolves the parameters from the dictionary to the correct types
        /// </summary>
        /// <param name="parameters">Parameter values and their types</param>
        /// <returns>Array of resolved parameters</returns>
        public object[] Resolve(Dictionary<Type, object> parameters)
        {
            // type is the type of the parameter
            // object is the value of the parameter, currently it will be JsonElement
            // the values should be converted to the correct type

            return parameters.Select(parameter => ((JsonElement)parameter.Value).Deserialize(parameter.Key)).ToArray();
        }
    }
}