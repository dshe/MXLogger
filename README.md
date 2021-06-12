# MXLogger&nbsp;&nbsp;[![Build status](https://ci.appveyor.com/api/projects/status/e51gaj9271kvpwhc?svg=true)](https://ci.appveyor.com/project/dshe/mxlogger) [![NuGet](https://img.shields.io/nuget/vpre/MXLogger.svg)](https://www.nuget.org/packages/MXLogger/) [![NuGet](https://img.shields.io/nuget/dt/MXLogger?color=orange)](https://www.nuget.org/packages/MXLogger/) [![License](https://img.shields.io/badge/license-Apache%202.0-7755BB.svg)](https://opensource.org/licenses/Apache-2.0)

***Minimalist Microsoft.Extensions.Logging Provider***
- compatible with xUnit, NUnit, MSTest and other test frameworks
- **.NET Standard 2.0** library
- customizable formatting
- supports scopes
- provides output caching
- supports Microsoft.Extensions.DependencyInjection
- dependencies: Microsoft.Extensions.Logging

### Installation ###
```csharp
PM> Install-Package MXLogger
```

### Simple Example (xUnit) ###
```csharp
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;

public class SimpleTest
{
    public readonly ILogger Logger;

    public SimpleTest(ITestOutputHelper output)
    {
        Logger = new LoggerFactory()
            .AddMXLogger(output.WriteLine)
            .CreateLogger("SimpleTest");
    }

    [Fact]
    public void Test()
    {
        Logger.LogInformation("Some message.");
        ...
    }
}
```
```csharp
xUnit output: "Info	  SimpleTest	  Some message."
```
### Base Class Example (xUnit) ###
```csharp
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

public abstract class BaseTest
{
    protected readonly ILogger Logger;
    protected IMyLibrary MyLibrary;

    public BaseTest(ITestOutputHelper output)
    {
        Logger = new LoggerFactory()
            .AddMXLogger(output.WriteLine)
            .CreateLogger("Test");
        
        MyLibrary = new MyLibrary(Logger);
    }
}
```
```csharp
using Microsoft.Extensions.Logging;
using Xunit;

public class Example : BaseTest
{
    public Example(ITestOutputHelper output) : base(output) { }

    [Fact]
    public void Test()
    {
        MyLibrary.Run();
        ...
    }
}
```
### Dependency Injection Example (xUnit) ###
```csharp
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

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
        MyComponent.Logger.LogCritical("Some message.");
        ...
    }
}
```
```csharp
xUnit output: "Crit	  Namespace.MyComponent	  Some message."
```
