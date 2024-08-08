using System;

namespace Resurrect.Resolvers
{
    public class SimpleFunctionResolver : IFunctionResolver
    {
        /// <summary>
        /// Resolves an instance of a function type using the default constructor. (No dependency injection supported)
        /// </summary>
        /// <param name="functionType"></param>
        /// <returns>Returns an instance of the function type</returns>
        public object ResolveInstance(Type functionType)
        {
            return Activator.CreateInstance(functionType);
        }
    }
}