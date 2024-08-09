# Resurrect

Resurrect is a package that allows you to serialize a function for storage or transmission. The serialized function can be resurrected and executed in a different environment.

## Table of Contents

- [Installation](#installation)
- [Features](#features)
- [Usage](#usage)
- [ASP.NET Core Usage](#aspnet-core-usage)
- [TODO](#todo)

## Installation

```bash
dotnet add package Resurrect
```

Or simply search for `Resurrect` in the NuGet gallery.

To use the package in ASP.NET Core projects, install the `Resurrect.AspNetCore` package.

```bash
dotnet add package Resurrect.AspNetCore
```

Or simply search for `Resurrect.AspNetCore` in the NuGet gallery.

## Features

- Serialize a function to any format (JSON, XML, binary, etc.)
- Resurrect the serialized function and execute it
- Supports functions with parameters
- Supports DI in the resurrected function
- Supports async functions
- Supports functions with return values

The execution environment can be different from the serialization environment. However it must have access to the assembly that contains the method and dependencies of the method.

<sup>The function is technically not serialized. Instead, all the information required to execute the function is serialized. This includes the method name, class name, assembly name, and parameters of the method. The function is resurrected by using reflection to find the method and class, and then invoking the method with the parameters.</sup>

## Usage

Start by serializing a function using the `SerializableFunction.Serialize` method. The method takes a lambda expression as an argument. The lambda expression should be a function call to the method you want to serialize. The method can be a static method or an instance method.

```csharp
var function = SerializableFunction.Serialize<TestService>(service => service.TestMethod("Hello, World!"));
var serializedFunction = JsonSerializer.Serialize(function);
```

The serialized function can be stored in a database, file, or transmitted over the network. To resurrect the function, use the `Resurrector` class.

```csharp
var deserializedFunction = JsonSerializer.Deserialize<SerializableFunction>(serializedFunction);

var resurrector = new Resurrector(new ResurrectionOptions
{
    FunctionResolver = new ServiceCollectionFunctionResolver(serviceProvider),
    ParameterResolver = new JsonParameterResolver()
});

resurrector.Invoke(deserializedFunction);
```

The `FunctionResolver` property of the `ResurrectionOptions` class is used to resolve the class instance of the method. The `ParameterResolver` property is used to resolve the parameters of the method. The `ServiceCollectionFunctionResolver` class is a built-in function resolver that uses the `IServiceProvider` to resolve the class instance. The `JsonParameterResolver` class is a built-in parameter resolver that uses JSON to resolve the parameters of the method.

**Note:** The `ServiceCollectionFunctionResolver` and `JsonParameterResolver` class requires the `Resurrect.AspNetCore` package.

## ASP.NET Core Usage

To use Resurrect in ASP.NET Core projects, add the `Resurrect.AspNetCore` package to your project. The package provides a built-in function resolver and parameter resolver for ASP.NET Core projects.

```csharp
services.AddResurrect();
```

The `AddResurrect` method adds the `ServiceCollectionFunctionResolver` and `JsonParameterResolver` to the DI container. The `Resurrector` class will automatically use these resolvers when resolving the function and parameters.

To use the `Resurrector` class in your controller, inject the `Resurrector` class into the controller.

```csharp
public class TestController : ApiController
{
    private readonly Resurrector _resurrector;

    public TestController(Resurrector resurrector)
    {
        _resurrector = resurrector;
    }

    [HttpPost]
    public IActionResult ExecuteFunction(SerializedFunction serializedFunction)
    {
        _resurrector.Invoke(deserializedFunction);
        return Ok();
    }
}
```

To use other resolvers, configure the `ResurrectionOptions` class in the `AddResurrect` method.

```csharp
services.AddResurrect(options =>
{
    options.FunctionResolver = new CustomFunctionResolver();
    options.ParameterResolver = new CustomParameterResolver();
});
```

## TODO

- [ ] Test coverage
- [ ] Handle versioning for the package better