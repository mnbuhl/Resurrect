using System;

namespace Resurrect
{
    public class ResurrectionOptions
    {
        // set IoC container
        public IServiceProvider ServiceProvider { get; set; }
        public IParameterTypeResolver ParameterTypeResolver { get; set; }
    }
}