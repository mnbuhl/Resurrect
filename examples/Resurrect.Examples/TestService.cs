namespace Resurrect.Examples;

public class TestService : ITestService
{
    private readonly LoggerService _loggerService;
    
    public TestService(LoggerService loggerService)
    {
        _loggerService = loggerService;
    }
    
    public void TestMethod(string message)
    {
        _loggerService.Log(message);
    }
    
    public async Task TestMethodAsync(Payload payload)
    {
        await _loggerService.LogAsync(payload.Method);
    }
    
    public string TestMethodWithReturnValue(string message)
    {
        _loggerService.Log(message);
        return message;
    }
    
    public async Task<Payload> TestMethodWithReturnValueAsync(Payload payload)
    {
        await _loggerService.LogAsync(payload.Method);
        return payload;
    }
}