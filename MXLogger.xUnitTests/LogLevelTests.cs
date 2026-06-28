namespace MXLogger.xUnitTests;

public class LogLevelTests
{
    private readonly Action<string> _writeLine;
    public LogLevelTests(ITestOutputHelper output) => _writeLine = output.WriteLine;

    private static void TestLogger(ILogger logger)
    {
        logger.LogCritical("Logger.LogCritical");
        logger.LogError("Logger.LogError");
        logger.LogWarning("Logger.LogWarning");
        logger.LogInformation("Logger.LogInformation");
        logger.LogDebug("Logger.LogDebug");
        logger.LogTrace("Logger.LogTrace");
        logger.Log(LogLevel.None, "test none");
    }

    [Fact]
    public void Test1()
    {
        ILoggerFactory factory = LoggerFactory
            .Create(builder => builder
                .AddMXLogger(_writeLine, MXLogFormatType.SingleLine)
                .SetMinimumLevel(LogLevel.Debug));

        ILogger logger1 = factory.CreateLogger("category");

        TestLogger(logger1);
    }
}
