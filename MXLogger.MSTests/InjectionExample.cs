using Microsoft.Extensions.DependencyInjection;
namespace MXLogger.MSTests;

public class MyComponent
{
    public readonly ILogger Logger;

    public MyComponent(ILogger<MyComponent> logger)
    {
        Logger = logger;
    }
}

[TestClass]
public class InjectionExample
{
    private static Action<string>? _writeLine;

    [ClassInitialize]
    public static void TestFixtureSetup(TestContext context)
    {
        _writeLine = context.WriteLine;
    }

    public MyComponent MyComponent { get; }

    public InjectionExample()
    {
        MyComponent = new ServiceCollection()
            .AddTransient<MyComponent>()
            .AddLogging(builder => builder
                .AddMXLogger(_writeLine!)
                .SetMinimumLevel(LogLevel.Trace))
            .BuildServiceProvider()
            .GetRequiredService<MyComponent>();
    }

    [TestMethod]
    public void Test()
    {
        MyComponent.Logger.LogCritical("message");
    }
}
