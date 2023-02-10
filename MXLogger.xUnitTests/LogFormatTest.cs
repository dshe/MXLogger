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
        private string? WriteFormat(MXLogInfo logInfo)
        {

            var str = LoggerProvider.Format(logInfo);
            if (str != null)
                Write(str);
            return str;
        }

        [Fact]
        public void LoggingFormatLogLevel()
        {
            WriteFormat(new MXLogInfo("category1", LogLevel.Critical,    new EventId(1, "ename"),       null, null, "text"));
            WriteFormat(new MXLogInfo("category2", LogLevel.Debug,       new EventId(42, "ename"),      null, null, "text"));
            WriteFormat(new MXLogInfo("category3", LogLevel.Error,       new EventId(421, "ename"),     null, null, "text"));
            WriteFormat(new MXLogInfo("category4", LogLevel.Information, new EventId(4262, "ename"),    null, null, "text"));
            WriteFormat(new MXLogInfo("category5", LogLevel.None,        new EventId(42123, "ename"),   null, null, "text"));
            WriteFormat(new MXLogInfo("category6", LogLevel.Trace,       new EventId(421234, "ename"),  null, null, "text"));
            WriteFormat(new MXLogInfo("category7", LogLevel.Warning,     new EventId(4212345, "ename"), null, null, "text"));
        }

        [Fact]
        public void LoggingFormatTest()
        {
            var logInfo = new MXLogInfo("category", 0, new EventId(), null, null, "text");
            Assert.Equal("Trace: category\r\ntext\r\n", WriteFormat(logInfo));
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
}
