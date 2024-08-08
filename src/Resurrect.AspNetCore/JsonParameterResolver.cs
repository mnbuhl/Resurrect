using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Resurrect.AspNetCore
{
    public class JsonParameterResolver : IParameterResolver
    {
        public object[] Resolve(Dictionary<Type, object> parameters)
        {
            // type is the type of the parameter
            // object is the value of the parameter, currently it will be JsonElement
            // the values should be converted to the correct type

            return parameters.Select(parameter => ((JsonElement)parameter.Value).Deserialize(parameter.Key)).ToArray();
        }
    }
}