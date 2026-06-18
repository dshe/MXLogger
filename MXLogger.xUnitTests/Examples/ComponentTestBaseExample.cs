namespace MXLogger.xUnitTests;

public class MyComponent2
{
    private readonly ILogger _logger;

    public MyComponent2(ILogger logger)
    {
        _logger = logger;
    }

    public void Run()
    {
        _logger.LogInformation("message");
        /* ... */
    }
}

public abstract class ComponentTestBase
{
    protected readonly ILogger<ComponentTestBase> Logger;
    protected readonly MyComponent2 MyComponent2;

    protected ComponentTestBase(ITestOutputHelper output)
    {
        ILoggerFactory factory = LoggerFactory
            .Create(builder => builder
                .AddMXLogger(output.WriteLine)
                .SetMinimumLevel(LogLevel.Debug));

        Logger = factory.CreateLogger<ComponentTestBase>();

        ILogger myComponentLogger = factory.CreateLogger<MyComponent2>();
        MyComponent2 = new MyComponent2(myComponentLogger);
    }
}

public class ComponentTestExample : ComponentTestBase
{
    public ComponentTestExample(ITestOutputHelper output) : base(output) { }

    [Fact]
    public void Test()
    {
        Logger.LogInformation("message");

        MyComponent2.Run();
    }
}
