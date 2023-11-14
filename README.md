# MXLogger&nbsp;&nbsp;[![Build status](https://ci.appveyor.com/api/projects/status/e51gaj9271kvpwhc?svg=true)](https://ci.appveyor.com/project/dshe/mxlogger) [![NuGet](https://img.shields.io/nuget/vpre/MXLogger.svg)](https://www.nuget.org/packages/MXLogger/) [![NuGet](https://img.shields.io/nuget/dt/MXLogger?color=orange)](https://www.nuget.org/packages/MXLogger/) [![License](https://img.shields.io/badge/license-Apache%202.0-7755BB.svg)](https://opensource.org/licenses/Apache-2.0)

***Minimalist Microsoft Extensions Logging Provider***
- **.NET Standard 2.0** library
- compatible with xUnit, NUnit, MSTest and other test frameworks
- customizable formatting
- supports scopes
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

namespace Xunit.Abstractions;

// This class may be used as the base class for test classes.
public abstract class XunitTestBase
{
    private readonly ITestOutputHelper Output;
    protected readonly ILogger Logger;
    protected void Write(string format, params object[] args) =>
        Output.WriteLine(string.Format(format, args);

    protected XunitTestBase(ITestOutputHelper output, LogLevel logLevel = LogLevel.Debug, string name = "Test")
    {
        Output = output;
        Logger = CreateLogger(logLevel, name);
    }

    protected ILogger CreateLogger(LogLevel logLevel, string name)
    {
        return LoggerFactory
            .Create(builder => builder
                .AddMXLogger(Output.WriteLine)
                .SetMinimumLevel(logLevel))
            .CreateLogger(name);
    }
}
```
```csharp
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;

namespace YourNamespace;

public class SimpleExample : XunitTestBase
{
    public SimpleExample(ITestOutputHelper output) : base(output) { }

    [Fact]
    public void Test()
    {
        Logger.LogInformation("message!");
        Write("test!");
    }
}
```
output:
```csharp
Info: Test
message!

test!
```
### Dependency Injection Example (xUnit) ###
```csharp
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

public class MyComponent
{
    private readonly ILogger Logger;
    
    public MyComponent(ILogger<MyComponent> logger)
    {
        Logger = logger;
    }
    
    public void Run()
    {
        Logger.LogCritical("Message");
        ...
    }    
}

public class DependencyInjectionTest
{
    private readonly MyComponent MyComponent;

    public DependencyInjectionTest(ITestOutputHelper output)
    {
        MyComponent = new ServiceCollection()
            .AddTransient<MyComponent>()
            .AddLogging(builder => builder
                .AddMXLogger(output.WriteLine)
                .SetMinimumLevel(LogLevel.Debug))
            .BuildServiceProvider()
            .GetRequiredService<MyComponent>();
    }

    [Fact]
    public void Test()
    {
        MyComponent.Run();
        ...
    }
}
```
output:
```csharp
Crit: MyNamespace.MyComponent
Message
```
