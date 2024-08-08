using System;
using System.Collections.Generic;

namespace Resurrect
{
    public interface IParameterResolver
    {
        object[] Resolve(Dictionary<Type, object> parameters);
    }
}