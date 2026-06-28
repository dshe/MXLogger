namespace MXLogger.xUnitTests;

public class LogFormatSingleLineTest
{
    private readonly Action<string> _write;
    private readonly MXLoggerProvider _loggerProvider;
    public LogFormatSingleLineTest(ITestOutputHelper output)
    {
        _write = output.WriteLine;
        _loggerProvider = new MXLoggerProvider(_write, MXLogFormatType.SingleLine);
    }
    private string? WriteFormat(MXLogInfo logInfo)
    {

        var str = _loggerProvider.Format(logInfo);
        if (str != null)
            _write(str);
        return str;
    }

    [Fact]
    public void LoggingFormatLogLevel()
    {
        WriteFormat(new MXLogInfo("category1", LogLevel.Critical, new EventId(1, "ename"), null, null, "text"));
        WriteFormat(new MXLogInfo("category2", LogLevel.Debug, new EventId(42, "ename"), null, null, "text"));
        WriteFormat(new MXLogInfo("category3", LogLevel.Error, new EventId(421, "ename"), null, null, "text"));
        WriteFormat(new MXLogInfo("category4", LogLevel.Information, new EventId(4262, "ename"), null, null, "text"));
        WriteFormat(new MXLogInfo("category5", LogLevel.None, new EventId(42123, "ename"), null, null, "text"));
        WriteFormat(new MXLogInfo("category6", LogLevel.Trace, new EventId(421234, "ename"), null, null, "text"));
        WriteFormat(new MXLogInfo("category7", LogLevel.Warning, new EventId(4212345, "ename"), null, null, "text"));
    }

    [Fact]
    public void LoggingFormatTest()
    {
        var logInfo = new MXLogInfo("class.category", 0, new EventId(), null, null, "text");
        Assert.Equal("T> category: text", WriteFormat(logInfo));
    }

    [Fact]
    public void LoggingFormatException()
    {
        try
        {
            throw new Exception("exception message");
        }
        catch (Exception exception)
        {
            WriteFormat(new MXLogInfo("category", 0, new EventId(), null, exception, "message"));
        }
    }
}
