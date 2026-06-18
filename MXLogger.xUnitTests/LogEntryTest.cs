namespace MXLogger.xUnitTests;

public class LogEntryTest
{
    private readonly Action<string> _writeLine;
    private MXLogInfo LastEntry => _loggerProvider.GetLogEntries().Last();
    private readonly MXLoggerProvider _loggerProvider;
    private readonly ILogger _logger;

    public LogEntryTest(ITestOutputHelper output)
    {
        _writeLine = output.WriteLine;
        _loggerProvider = new MXLoggerProvider(_writeLine);
        ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder.AddProvider(_loggerProvider));
        _logger = loggerFactory.CreateLogger("category");
    }

    [Fact]
    public void LoggingLevelTest()
    {
        MXLoggerProvider loggerProvider = new(_writeLine, LogLevel.Warning);
        ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder.AddProvider(loggerProvider));
        ILogger logger = loggerFactory.CreateLogger("category");

        logger.LogTrace("trace message");
        logger.LogDebug("debug message");
        logger.LogInformation("information message");
        logger.LogWarning("warning message");
        logger.LogError("error message");
        logger.LogCritical("critical message");
        Assert.Equal(3, loggerProvider.GetLogEntries().Count);
    }

    [Fact]
    public void LoggingInfoTest()
    {
        _logger.LogCritical("message");

        Assert.Equal("category", LastEntry.Category);
        Assert.Equal(LogLevel.Critical, LastEntry.LogLevel);
        Assert.Equal(0, LastEntry.EventId.Id);

        IEnumerable<KeyValuePair<string, object>>? properties = LastEntry.State as IEnumerable<KeyValuePair<string, object>>;
        Assert.NotNull(properties);
        var property = properties!.Single();
        Assert.Equal("{OriginalFormat}", property.Key);
        Assert.Equal("message", property.Value);

        Assert.Null(LastEntry.Exception);
        Assert.Equal("message", LastEntry.Text);
    }

    [Fact]
    public void LoggingExceptionTest()
    {
        Exception exception = new Exception("exception message");
        _logger.LogInformation(exception, "message");

        Assert.Equal("category", LastEntry.Category);
        Assert.Equal(LogLevel.Information, LastEntry.LogLevel);
        Assert.Equal(0, LastEntry.EventId.Id);

        var properties = LastEntry.State as IEnumerable<KeyValuePair<string, object>>;
        Assert.NotNull(properties);
        var property = properties!.Single();
        Assert.Equal("{OriginalFormat}", property.Key);
        Assert.Equal("message", property.Value);

        Assert.Equal(exception, LastEntry.Exception);
        Assert.Equal("message", LastEntry.Text);
    }
}
