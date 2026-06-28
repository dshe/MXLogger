using System.Data;
using Microsoft.Extensions.DependencyInjection;
namespace MXLogger.xUnitTests;

public class LoggerFormatTest
{
    private readonly Action<string> _writeLine;
    public LoggerFormatTest(ITestOutputHelper output) => _writeLine = output.WriteLine;

    [Fact]
    public void InjectionTest()
    {
        MXLoggerProvider loggerProvider = new(_writeLine);
        IServiceCollection services = new ServiceCollection().AddLogging(builder => builder.AddProvider(loggerProvider));
        ServiceProvider serviceProvider = services.BuildServiceProvider();

        ILoggerFactory factory = serviceProvider.GetRequiredService<ILoggerFactory>()!;
        ILogger logger1 = factory.CreateLogger("category");
        logger1.LogInformation("test");
        Assert.Equal("test", loggerProvider.GetLogEntries().Last().Text);

        ILogger<LoggerFormatTest> logger2 = serviceProvider.GetRequiredService<ILogger<LoggerFormatTest>>();
        logger2!.LogInformation("test");
        Assert.Equal("test", loggerProvider.GetLogEntries().Last().Text);
    }

    [Fact]
    public void InjectionWithExtensionTest()
    {
        IServiceCollection services = new ServiceCollection()
            .AddLogging(builder => builder
            .AddMXLogger(_writeLine)
            .SetMinimumLevel(LogLevel.Debug));
        ServiceProvider serviceProvider = services.BuildServiceProvider();
        ILogger<LoggerFormatTest> logger = serviceProvider.GetRequiredService<ILogger<LoggerFormatTest>>();
        logger!.LogInformation("test");
    }

    [Fact]
    public void LoggingFactoryTest()
    {
        MXLoggerProvider loggerProvider = new(_writeLine);
        ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddProvider(loggerProvider));
        ILogger<LoggerFormatTest> logger = factory.CreateLogger<LoggerFormatTest>();
        logger.LogInformation("anything");
        Assert.Equal("MXLogger.xUnitTests.LoggerFormatTest", loggerProvider.GetLogEntries().Last().Category);
        factory.Dispose();
    }

    [Fact]
    public void LoggingFactoryWithExtensionTest()
    {
        ILoggerFactory factory = LoggerFactory
            .Create(builder => builder
                .AddMXLogger(_writeLine)
                .SetMinimumLevel(LogLevel.Debug));
        ILogger<LoggerFormatTest> logger = factory.CreateLogger<LoggerFormatTest>();
        logger.LogInformation("anything");
        factory.Dispose();
    }

    [Fact]
    public void CallerNameTest()
    {
        MXLoggerProvider loggerProvider = new(_writeLine);
        using ILoggerFactory factory = LoggerFactory.Create(x => x.AddProvider(loggerProvider));
        ILogger logger = factory.CreateLoggerFromCallerMemberName();
        logger.LogInformation("anything");
        Assert.Equal("CallerNameTest", loggerProvider.GetLogEntries().Last().Category);
    }

    [Fact]
    public void TestScopes()
    {
        MXLoggerProvider loggerProvider = new(_writeLine);
        ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddProvider(loggerProvider));
        ILogger<LoggerFormatTest> logger = factory.CreateLogger<LoggerFormatTest>();

        logger.LogError("outsideLoop");
        using (logger.BeginScope("scope1"))
        {
            logger.LogError("loop1");
            using (logger.BeginScope("scope2"))
            {
                logger.LogError("loop2");
                using (logger.BeginScope("scope3"))
                {
                    logger.LogError("loop3");
                }
                logger.LogError("loop2");
            }
            logger.LogError("loop1");
        }
        factory.Dispose();
    }
}
