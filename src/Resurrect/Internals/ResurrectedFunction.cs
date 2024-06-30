using System;
using System.Collections.Generic;
using System.Reflection;

namespace Resurrect.Internals
{
    internal class ResurrectedFunction
    {
        public Type Type { get; set; }
        public MethodInfo Method { get; set; }
        public Dictionary<string, object> Parameters { get; set; } = new Dictionary<string, object>();
    }
}