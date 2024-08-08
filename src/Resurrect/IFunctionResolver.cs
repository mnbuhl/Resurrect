using System;

namespace Resurrect
{
    /// <summary>
    /// Interface for resolving instances of function types.
    /// </summary>
    public interface IFunctionResolver
    {
        /// <summary>
        /// Resolves an instance of a function type using the ResurrectConstructor attribute or the constructor with the most parameters.
        /// </summary>
        /// <param name="functionType">The type of the function to resolve</param>
        /// <returns>Returns an instance of the function type</returns>
        object ResolveInstance(Type functionType);
    }
}