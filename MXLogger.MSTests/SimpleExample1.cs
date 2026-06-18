namespace MXLogger.MSTests;

[TestClass]
public class SimpleExample
{
    private static Action<string>? _writeLine;

    [ClassInitialize]
    public static void TestFixtureSetup(TestContext context)
    {
        _writeLine = context.WriteLine;
    }

    private readonly ILogger _logger;

    public SimpleExample()
    {
        Assert.IsNotNull(_writeLine);

        ILoggerFactory factory = LoggerFactory
            .Create(builder => builder
                .AddMXLogger(_writeLine)
                .SetMinimumLevel(LogLevel.Trace));

        _logger = factory.CreateLogger("CategoryName");
    }

    [TestMethod]
    public void Test()
    {
        _logger.LogInformation("message");
    }
}
