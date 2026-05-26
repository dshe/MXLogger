namespace MXLogger.MSTests;

[TestClass]
public class SimpleExample
{
    private static Action<string>? WriteLine;

    [ClassInitialize]
    public static void TestFixtureSetup(TestContext context)
    {
        WriteLine = context.WriteLine;
    }

    private readonly ILogger Logger;

    public SimpleExample()
    {
        Assert.IsNotNull(WriteLine);

        ILoggerFactory factory = LoggerFactory
            .Create(builder => builder
                .AddMXLogger(WriteLine)
                .SetMinimumLevel(LogLevel.Trace));

        Logger = factory.CreateLogger("CategoryName");
    }

    [TestMethod]
    public void Test()
    {
        Logger.LogInformation("message");
    }
}
