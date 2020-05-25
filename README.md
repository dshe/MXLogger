## MXLogger&nbsp;&nbsp;[![Build status](https://ci.appveyor.com/api/projects/status/e51gaj9271kvpwhc?svg=true)](https://ci.appveyor.com/project/dshe/mxlogger) [![NuGet](https://img.shields.io/nuget/vpre/MXLogger.svg)](https://www.nuget.org/packages/MXLogger/) [![License](https://img.shields.io/badge/license-Apache%202.0-7755BB.svg)](https://opensource.org/licenses/Apache-2.0)

***Minimalist Microsoft.Extensions.Logging Provider***
- compatible with xUnit and other test frameworks
- NetStandard 2.0 library
- fully customizable formatting
- provides output caching
- dependencies: Microsoft.Extensions.Logging

**Example**

```csharp
using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;
using MXLogger;

public class LoggerTest
{
    private readonly ILogger Logger;

    public LoggerTest(ITestOutputHelper output)
    {
        var loggerFactory = new LoggerFactory().AddMXLogger(output.WriteLine);
        Logger = loggerFactory.CreateLogger("CategoryName");
    }

    [Fact]
    public void Test()
    {
        Logger.LogInformation("message");
    }
}

public class LoggerDependencyInjectionTest
{
    protected readonly IServiceProvider ServiceProvider;

    public LoggerDependencyInjectionTest(ITestOutputHelper output)
    {
        IServiceCollection services = new ServiceCollection();
        services.AddLogging(builder => builder.AddMXLogger(output.WriteLine));
        ServiceProvider = services.BuildServiceProvider();
    }

    [Fact]
    public void Test()
    {
        var logger = ServiceProvider.GetService<ILogger<LoggerTest>>();
        logger.LogCritical("message");
    }
}
```
