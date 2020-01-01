using Microsoft.Extensions.Logging;
using System;
using Xunit;
using Xunit.Abstractions;
using MXLogger;



namespace MXLoggerTest
{
    public class LogFormatTest
    {
        private readonly Action<string> Write;
        private readonly MXLoggerProvider LoggerProvider;
        public LogFormatTest(ITestOutputHelper output)
        {
            Write = output.WriteLine;
            LoggerProvider = new MXLoggerProvider(Write);
        }
        private string? Format(LogInfo logInfo)
        {

            var str = LoggerProvider.Format(logInfo);
            if (str != null)
                Write(str);
            return str;
        }

        [Fact]
        public void LoggingFormatLogLevel()
        {
            Format(new LogInfo("category",    LogLevel.Critical,    new EventId(1, "name"),       null, null, "text"));
            Format(new LogInfo("category1",   LogLevel.Debug,       new EventId(42, "namex"),     null, null, "text"));
            Format(new LogInfo("category123", LogLevel.Error,       new EventId(421, "nameyz"),   null, null, "text"));
            Format(new LogInfo("categoiweiy", LogLevel.Information, new EventId(4262, "name"),    null, null, "text"));
            Format(new LogInfo("category",    LogLevel.None,        new EventId(42123, "name"),   null, null, "text"));
            Format(new LogInfo("category",    LogLevel.Trace,       new EventId(421234, "name"),  null, null, "text"));
            Format(new LogInfo("category",    LogLevel.Warning,     new EventId(4212345, "name"), null, null, "text"));
        }

        [Fact]
        public void LoggingFormatTest()
        {
            var logInfo = new LogInfo("category", 0, new EventId(), null, null, null);
            Assert.Equal("Trace\t  category\t  ", Format(logInfo));
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
                Format(new LogInfo("category", 0, new EventId(), null, exception, "message"));
            }
        }
    }
}
