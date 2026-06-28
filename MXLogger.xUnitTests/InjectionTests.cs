using Microsoft.Extensions.DependencyInjection;
namespace MXLogger.xUnitTests;

public class InjectionTests
{
    private readonly Action<string> _writeLine;
    public InjectionTests(ITestOutputHelper output)
    {
        _writeLine = output.WriteLine;
    }

    [Fact]
    public void Test()
    {
        MXLoggerProvider provider = new MXLoggerProvider(_writeLine);

        ServiceProvider serviceProvider = new ServiceCollection()
            .AddLogging(builder => builder.AddProvider(provider))
            .AddTransient<Component1>()
            .AddTransient<Component2>()
            .BuildServiceProvider();

        // inject ILogger<T>
        Component1 component1 = serviceProvider.GetRequiredService<Component1>()!;
        component1.Log("test");
        Assert.Equal("Info:  MXLogger.xUnitTests.Component1\r\ntest\r\n", provider.Format(provider.GetLogEntries().Last()));

        // inject ILoggerFactory
        Component2 component2 = serviceProvider.GetRequiredService<Component2>()!;
        component2.Log("test");
        Assert.Equal("Info:  Component2Name\r\ntest\r\n", provider.Format(provider.GetLogEntries().Last()));
    }
}

public class Component1
{
    private readonly ILogger _logger;

    public Component1(ILogger<Component1> logger)
    {
        _logger = logger;
    }

    public void Log(string s)
    {
        _logger.LogInformation(s);
    }
 }

public class Component2
{
    private readonly ILogger _logger;

    public Component2(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger("Component2Name");
    }

    public void Log(string s)
    {
        _logger.LogInformation(s);
    }
}
