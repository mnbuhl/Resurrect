// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Resurrect;
using Resurrect.AspNetCore.Extensions;
using Resurrect.Examples;

var serviceCollection = new ServiceCollection();

serviceCollection.AddResurrect();
serviceCollection.AddSingleton<LoggerService>();
serviceCollection.AddSingleton<ITestService, TestService>();

var serviceProvider = serviceCollection.BuildServiceProvider();

// var resurrector = new Resurrector(new ResurrectionOptions
// {
//     FunctionResolver = new ServiceCollectionFunctionResolver(serviceCollection.BuildServiceProvider()),
//     ParameterTypeResolver = new JsonParameterTypeResolver()
// });

var resurrector = serviceProvider.GetRequiredService<Resurrector>();

var function1 = Resurrector.Serialize<ITestService>(service => service.TestMethod("Hello, World 1!"));
var serializedFunction1 = JsonSerializer.Serialize(function1);
var deserializedFunction1 = JsonSerializer.Deserialize<SerializableFunction>(serializedFunction1);
resurrector.Invoke(deserializedFunction1);

var function2 = Resurrector.Serialize<TestService>(service => service.TestMethodAsync(new Payload("Hello, World 2!")));
var serializedFunction2 = JsonSerializer.Serialize(function2);
var deserializedFunction2 = JsonSerializer.Deserialize<SerializableFunction>(serializedFunction2);
await resurrector.InvokeAsync(deserializedFunction2);

var function3 = Resurrector.Serialize<TestService>(service => service.TestMethodWithReturnValue("Hello, World 3!"));
var serializedFunction3 = JsonSerializer.Serialize(function3);
var deserializedFunction3 = JsonSerializer.Deserialize<SerializableFunction>(serializedFunction3);
var result = resurrector.Invoke<string>(deserializedFunction3);
Console.WriteLine(result);

var function4 = Resurrector.Serialize<ITestService>(service => service.TestMethodWithReturnValueAsync(new Payload("Hello, World 4!")));
var serializedFunction4 = JsonSerializer.Serialize(function4);
var deserializedFunction4 = JsonSerializer.Deserialize<SerializableFunction>(serializedFunction4);
var result2 = await resurrector.InvokeAsync<Payload>(deserializedFunction4);
Console.WriteLine(result2.Method);