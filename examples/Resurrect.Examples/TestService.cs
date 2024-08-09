namespace Resurrect.Examples;

public class TestService(LoggerService loggerService) : ITestService
{
    public void TestMethod(string message)
    {
        loggerService.Log(message);
    }
    
    public async Task TestMethodAsync(Payload payload)
    {
        await loggerService.LogAsync(payload.Method);
    }
    
    public string TestMethodWithReturnValue(string message)
    {
        loggerService.Log(message);
        return message;
    }
    
    public async Task<Payload> TestMethodWithReturnValueAsync(Payload payload)
    {
        await loggerService.LogAsync(payload.Method);
        return payload;
    }
}