// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Resurrect;
using Resurrect.Examples;

var serviceCollection = new ServiceCollection();

serviceCollection.AddSingleton<LoggerService>();
serviceCollection.AddSingleton<TestService>();

var serviceProvider = serviceCollection.BuildServiceProvider();

var resurrector = new Resurrector(new ResurrectionOptions
{
    ServiceProvider = serviceProvider
});

var function1 = resurrector.ToFunction<TestService>(service => service.TestMethod("Hello, World!"));
var serializedFunction1 = JsonSerializer.Serialize(function1);
var deserializedFunction1 = JsonSerializer.Deserialize<Function>(serializedFunction1);
resurrector.Invoke(deserializedFunction1);

var function2 = resurrector.ToFunction<TestService>(service => service.TestMethodAsync(new Payload("Hello, World!")));
var serializedFunction2 = JsonSerializer.Serialize(function2);
var deserializedFunction2 = JsonSerializer.Deserialize<Function>(serializedFunction2);
await resurrector.InvokeAsync(deserializedFunction2);

var function3 = resurrector.ToFunction<TestService>(service => service.TestMethodWithReturnValue("Hello, World!"));
var serializedFunction3 = JsonSerializer.Serialize(function3);
var deserializedFunction3 = JsonSerializer.Deserialize<Function>(serializedFunction3);
var result = resurrector.Invoke<string>(deserializedFunction3);
Console.WriteLine(result);

var function4 = resurrector.ToFunction<TestService>(service => service.TestMethodWithReturnValueAsync(new Payload("Hello, World!")));
var serializedFunction4 = JsonSerializer.Serialize(function4);
var deserializedFunction4 = JsonSerializer.Deserialize<Function>(serializedFunction4);
var result2 = await resurrector.InvokeAsync<Payload>(deserializedFunction4);
Console.WriteLine(result2.Method);