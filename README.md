## MXLogger&nbsp;&nbsp;[![Build status](https://ci.appveyor.com/api/projects/status/e51gaj9271kvpwhc?svg=true)](https://ci.appveyor.com/project/dshe/mxlogger) [![NuGet](https://img.shields.io/nuget/vpre/MXLogger.svg)](https://www.nuget.org/packages/MXLogger/) [![License](https://img.shields.io/badge/license-Apache%202.0-7755BB.svg)](https://opensource.org/licenses/Apache-2.0)

***Minimalist Microsoft.Extensions.Logging Provider***
- compatible with xUnit and other test frameworks
- NetStandard 2.0 library
- customizable formatting
- provides output caching
- supports scopes
- supports Microsoft.Extensions.DependencyInjection
- dependencies: Microsoft.Extensions.Logging


### Installation ###
```csharp
PM> Install-Package MXLogger
```

### Simple Example ###

```csharp
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;
using MXLogger;

public class SimpleTest
{
    public readonly ILogger Logger;

    public SimpleTest(ITestOutputHelper output)
    {
        ILoggerFactory loggerFactory = new LoggerFactory().AddMXLogger(output.WriteLine);
        Logger = loggerFactory.CreateLogger("CategoryName");
    }

    [Fact]
    public void Test()
    {
        Logger.LogInformation("message");
    }
}
```
```csharp
Xunit output: "Info	  CategoryName	  message"
```
### Dependency Injection Example ###
```csharp
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;
using MXLogger;

public class MyComponent
{
    public readonly ILogger Logger;
    
    public MyComponent(ILogger<MyComponent> logger)
    {
        Logger = logger;
    }
}

public class DependencyInjectionTest
{
    public readonly MyComponent MyComponent;

    public DependencyInjectionTest(ITestOutputHelper output)
    {
        MyComponent = new ServiceCollection()
            .AddTransient<MyComponent>()
            .AddLogging(builder => builder.AddMXLogger(output.WriteLine))
            .BuildServiceProvider()
            .GetService<MyComponent>();
    }

    [Fact]
    public void Test()
    {
        MyComponent.Logger.LogCritical("message");
    }
}
```
```csharp
Xunit output: "Crit	  Namespace.MyComponent	  message"
```
