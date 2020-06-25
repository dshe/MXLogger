## MXLogger&nbsp;&nbsp;[![Build status](https://ci.appveyor.com/api/projects/status/e51gaj9271kvpwhc?svg=true)](https://ci.appveyor.com/project/dshe/mxlogger) [![NuGet](https://img.shields.io/nuget/vpre/MXLogger.svg)](https://www.nuget.org/packages/MXLogger/) [![License](https://img.shields.io/badge/license-Apache%202.0-7755BB.svg)](https://opensource.org/licenses/Apache-2.0)

***Minimalist Microsoft.Extensions.Logging Provider***
- compatible with xUnit and other test frameworks
- NetStandard 2.0 library
- customizable formatting
- provides output caching
- supports scopes
- supports Microsoft.Extensions.DependencyInjection
- dependencies: Microsoft.Extensions.Logging

**Examples**

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
        var loggerFactory = new LoggerFactory().AddMXLogger(output.WriteLine);
        Logger = loggerFactory.CreateLogger<SimpleTest>();
    }

    [Fact]
    public void Test()
    {
        Logger.LogInformation("message");
    }
}

```

```csharp
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;
using MXLogger;

public class MyDependency
{
    public readonly ILogger<MyDependency> Logger;
    
    public MyDependency(ILogger<MyDependency> logger)
    {
        Logger = logger;
    }
}

public class DependencyInjectionTest
{
    public readonly MyDependency MyDependency;

    public DependencyInjectionTest(ITestOutputHelper output)
    {
        MyDependency = new ServiceCollection()
            .AddTransient<MyDependency>()
            .AddLogging(builder => builder.AddMXLogger(output.WriteLine))
            .BuildServiceProvider()
            .GetService<MyDependency>();
    }

    [Fact]
    public void Test()
    {
        MyDependency.Logger.LogCritical("message");
    }
}
```
