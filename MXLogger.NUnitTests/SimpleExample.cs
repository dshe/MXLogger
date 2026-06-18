namespace MXLogger.NUnitTests;

public class SimpleExample
{
    private ILogger _logger;

    public SimpleExample()
    {
        ILoggerFactory factory = LoggerFactory
            .Create(builder => builder
                .AddMXLogger(TestContext.WriteLine)
                .SetMinimumLevel(LogLevel.Trace));

        _logger = factory.CreateLogger("CategoryName");
    }

    [Test]
    public void Test()
    {
        _logger.LogInformation("message");
    }
}
