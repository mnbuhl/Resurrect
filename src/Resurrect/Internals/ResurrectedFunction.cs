using System;
using System.Reflection;

namespace Resurrect.Internals
{
    internal class ResurrectedFunction
    {
        public Type Type { get; set; }
        public MethodInfo Method { get; set; }
        public object[] Parameters { get; set; } = Array.Empty<object>();
    }
}