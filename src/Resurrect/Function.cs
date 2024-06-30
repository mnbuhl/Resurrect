using System.Collections.Generic;

namespace Resurrect
{
    public class Function
    {
        public string Type { get; set; } = string.Empty;
        public string Method { get; set; } = string.Empty;
        public Dictionary<string, object> Parameters { get; set; } = new Dictionary<string, object>();
    }
}