using System;
using System.Collections.Generic;

namespace Resurrect
{
    public interface IParameterTypeResolver
    {
        object[] Resolve(Dictionary<Type, object> parameters);
    }
}