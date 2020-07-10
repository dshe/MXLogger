using System;
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;

namespace MXLogger.xUnitTests
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
        private string? WriteFormat(LogInfo logInfo)
        {

            var str = LoggerProvider.Format(logInfo);
            if (str != null)
                Write(str);
            return str;
        }

        [Fact]
        public void LoggingFormatLogLevel()
        {
            WriteFormat(new LogInfo("category1", LogLevel.Critical,    new EventId(1, "ename"),       null, null, "text"));
            WriteFormat(new LogInfo("category2", LogLevel.Debug,       new EventId(42, "ename"),      null, null, "text"));
            WriteFormat(new LogInfo("category3", LogLevel.Error,       new EventId(421, "ename"),     null, null, "text"));
            WriteFormat(new LogInfo("category4", LogLevel.Information, new EventId(4262, "ename"),    null, null, "text"));
            WriteFormat(new LogInfo("category5", LogLevel.None,        new EventId(42123, "ename"),   null, null, "text"));
            WriteFormat(new LogInfo("category6", LogLevel.Trace,       new EventId(421234, "ename"),  null, null, "text"));
            WriteFormat(new LogInfo("category7", LogLevel.Warning,     new EventId(4212345, "ename"), null, null, "text"));
        }

        [Fact]
        public void LoggingFormatTest()
        {
            var logInfo = new LogInfo("category", 0, new EventId(), null, null, null);
            Assert.Equal("Trace\t  category\t  ", WriteFormat(logInfo));
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
                WriteFormat(new LogInfo("category", 0, new EventId(), null, exception, "message"));
            }
        }
    }
}
