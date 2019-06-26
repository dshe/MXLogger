## MXLogger&nbsp;&nbsp; 
[![Build status](https://ci.appveyor.com/api/projects/status/e51gaj9271kvpwhc?svg=true)](https://ci.appveyor.com/project/dshe/mxlogger)
[![NuGet](https://img.shields.io/nuget/vpre/MXLogger.svg)](https://www.nuget.org/packages/MXLogger/) [![License](https://img.shields.io/badge/license-Apache%202.0-7755BB.svg)](https://opensource.org/licenses/Apache-2.0)

***Minimalist Microsoft.Extensions.Logging Provider***
- compatible with xUnit and other test frameworks
- supports Microsoft.Extensions.DependencyInjection
- NetStandard 2.0 library
- fully customizable formatting
- provides output caching
- dependencies: Microsoft.Extensions.Logging, Microsoft.Extensions.DependencyInjection.Abstractions

**Example**

```csharp
public class LoggerTest
{
    private readonly Action<string> WriteLine;
    public LoggerTest(ITestOutputHelper output) => WriteLine = output.WriteLine;

    [Fact]
    public void FactoryTest()
    {
        ILoggerFactory factory = new LoggerFactory().AddXLogger(WriteLine);
        ILogger logger = factory.CreateLogger<LoggerTest>();

        logger.LogCritical("message");
    }

    [Fact]
    public void InjectionTest()
    {
        IServiceCollection services = new ServiceCollection();
        services.AddLogging(builder => builder.AddXLogger(WriteLine));
        IServiceProvider serviceProvider = services.BuildServiceProvider();
        ILogger logger = serviceProvider.GetService<ILogger<LoggerTest>>();

        logger.LogInformation("message");
    }
}
```
