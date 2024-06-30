namespace Resurrect.Examples;

public class Payload
{
    public Payload(string method)
    {
        Method = method;
    }
    
    public string Method { get; set; }
}