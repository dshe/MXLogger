using Microsoft.Extensions.DependencyInjection;
namespace MXLogger.xUnitTests;

public class MyComponent1
{
    private readonly ILogger _logger;

    public MyComponent1(ILogger<MyComponent1> logger)
    {
        _logger = logger;
    }

    public void Run()
    {
        _logger.LogCritical("Test!");
    }
}

public class InjectionExample
{
    private readonly MyComponent1 _myComponent1;

    public InjectionExample(ITestOutputHelper output)
    {
        _myComponent1 = new ServiceCollection()
            .AddTransient<MyComponent1>()
            .AddLogging(builder => builder
                .AddMXLogger(output.WriteLine)
                .SetMinimumLevel(LogLevel.Debug))
            .BuildServiceProvider()
            .GetRequiredService<MyComponent1>()!;
    }

    [Fact]
    public void Test()
    {
        _myComponent1.Run();
    }
}
