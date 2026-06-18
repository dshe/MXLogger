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
    private readonly Action<string> _writeLine;
    public CustomLogFormatTest(ITestOutputHelper output)
    {
        _writeLine = output.WriteLine;
    }

    [Fact]
    public void LoggingFactoryTest()
    {
        MyCustomLoggerProvider provider = new(_writeLine);
        ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddProvider(provider));
        ILogger<CustomLogFormatTest> logger = factory.CreateLogger<CustomLogFormatTest>();
        
        logger.LogInformation("test");

        Assert.Equal("custom: test", provider.Format(provider.GetLogEntries().Last()));
        factory.Dispose();
    }
}
