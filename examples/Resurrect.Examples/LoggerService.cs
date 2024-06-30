namespace Resurrect.Examples;

public class LoggerService
{
    public void Log(string message)
    {
        Console.WriteLine(message);
    }
    
    public Task LogAsync(string message)
    {
        Console.WriteLine(message);
        return Task.CompletedTask;
    }
}