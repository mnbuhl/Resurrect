using System;
using System.Collections.Generic;

namespace Resurrect
{
    /// <summary>
    /// Interface for resolving the type of parameters for a function.
    /// </summary>
    public interface IParameterResolver
    {
        /// <summary>
        /// Resolves the parameters for a function using the provided parameters.
        /// </summary>
        /// <param name="parameters">Function parameters type and value</param>
        /// <returns>Returns an array of parameters</returns>
        object[] Resolve(Dictionary<Type, object> parameters);
    }
}