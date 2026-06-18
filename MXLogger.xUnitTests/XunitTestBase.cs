namespace Xunit.Abstractions;

public abstract class XunitTestBase
{
    private readonly ITestOutputHelper _output;
    protected readonly ILoggerFactory LogFactory;
    protected readonly ILogger Logger;
    protected void Write(string format, params object[] args) =>
        _output.WriteLine(string.Format(format, args) + Environment.NewLine);

    protected XunitTestBase(ITestOutputHelper output, LogLevel logLevel = LogLevel.Debug, string name = "Test")
    {
        _output = output;
        LogFactory = LoggerFactory.Create(builder => builder
            .AddMXLogger(output.WriteLine)
            .SetMinimumLevel(logLevel));
        Logger = LogFactory.CreateLogger(name);
    }
}
