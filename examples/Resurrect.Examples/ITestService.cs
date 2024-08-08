namespace Resurrect.Examples;

public interface ITestService
{
    void TestMethod(string message);
    Task TestMethodAsync(Payload payload);
    string TestMethodWithReturnValue(string message);
    Task<Payload> TestMethodWithReturnValueAsync(Payload payload);
}