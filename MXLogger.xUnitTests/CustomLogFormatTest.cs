namespace MXLogger.xUnitTests;

public class MyCustomLoggerProvider : MXLoggerProvider
{
    public MyCustomLoggerProvider(Action<string> writeLine) : base(writeLine) { }
    public override string? Format(MXLogInfo logInfo)
    {
        // create custom format here
        return "custom: " + logInfo.Text;
    }
}

public class CustomLogFormatTest
{
    internal readonly MyCustomLoggerProvider MyCustomLoggerProvider;
    public readonly ILoggerFactory MyLoggerFactory;
    public CustomLogFormatTest(ITestOutputHelper output)
    {
        MyCustomLoggerProvider = new(output.WriteLine);
        MyLoggerFactory = LoggerFactory.Create(builder => builder.AddProvider(MyCustomLoggerProvider));
    }

    [Fact]
    public void LoggingFactoryTest()
    {
        ILogger<CustomLogFormatTest> logger = MyLoggerFactory.CreateLogger<CustomLogFormatTest>();

        logger.LogInformation("test");

        Assert.StartsWith("custom: test", MyCustomLoggerProvider.Format(MyCustomLoggerProvider.GetLogEntries().Last()));
    }
}
