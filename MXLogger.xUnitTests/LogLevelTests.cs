namespace MXLogger.xUnitTests;

public class LogLevelTests
{
    private readonly Action<string> _writeLine;
    public LogLevelTests(ITestOutputHelper output) => _writeLine = output.WriteLine;

    private static void TestLogger(ILogger logger)
    {
        //WriteLine("WriteLine");
        //WriteLine("WriteLine");
        logger.LogTrace("test trace");
        logger.LogDebug("test debug");
        logger.LogInformation("test information");
        logger.LogWarning("test warning");
        logger.LogError("test error");
        logger.LogCritical("test critical");
        logger.Log(LogLevel.None, "test none");
    }

    [Fact]
    public void Test1()
    {
        List<string> strings = new();

        ILoggerFactory factory = LoggerFactory
            .Create(builder => builder
                .AddMXLogger(_writeLine)
                .SetMinimumLevel(LogLevel.Debug));

        ILogger logger1 = factory.CreateLogger("category");

        TestLogger(logger1);
    }
}
