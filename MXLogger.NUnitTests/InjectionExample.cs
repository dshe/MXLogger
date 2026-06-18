using Microsoft.Extensions.DependencyInjection;
namespace MXLogger.NUnitTests;

public class MyComponent
{
    public readonly ILogger Logger;

    public MyComponent(ILogger<MyComponent> logger)
    {
        Logger = logger;
    }
}

public class InjectionExample
{
    private MyComponent _myComponent;

    public InjectionExample()
    {
        _myComponent = new ServiceCollection()
            .AddTransient<MyComponent>()
            .AddLogging(builder => builder
                .AddMXLogger(TestContext.WriteLine)
                .SetMinimumLevel(LogLevel.Trace))
            .BuildServiceProvider()
            .GetRequiredService<MyComponent>()!;
    }

    [Test]
    public void Test()
    {
        _myComponent.Logger.LogCritical("message");
    }
}
