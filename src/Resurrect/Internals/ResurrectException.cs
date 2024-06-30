using System;

namespace Resurrect.Internals
{
    public class ResurrectException : Exception
    {
        public ResurrectException(string message) : base(message)
        {
        }
    }
}