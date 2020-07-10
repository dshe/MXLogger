using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;

namespace MXLogger.xUnitTests
{
    public class LogEntryTest
    {
        private readonly Action<string> WriteLine;
        private LogInfo LastEntry => LoggerProvider.GetLogEntries().LastOrDefault();
        private readonly MXLoggerProvider LoggerProvider;
        private readonly ILogger Logger;

        public LogEntryTest(ITestOutputHelper output)
        {
            WriteLine = output.WriteLine;
            LoggerProvider = new MXLoggerProvider(WriteLine);
            var loggerFactory = new LoggerFactory(new[] { LoggerProvider });
            Logger = loggerFactory.CreateLogger("category");
        }

        [Fact]
        public void LoggingLevelTest()
        {
            var loggerProvider = new MXLoggerProvider(WriteLine, LogLevel.Warning);
            var loggerFactory = new LoggerFactory(new[] { loggerProvider });
            var logger = loggerFactory.CreateLogger("category");

            logger.LogTrace("trace message");
            logger.LogDebug("debug message");
            logger.LogInformation("information message");
            logger.LogWarning("warning message");
            logger.LogError("error message");
            logger.LogCritical("critical message");
            Assert.Equal(3, loggerProvider.GetLogEntries().Count);
        }

        [Fact]
        public void LoggingInfoTest()
        {
            Logger.LogCritical("message");

            Assert.Equal("category", LastEntry.Category);
            Assert.Equal(LogLevel.Critical, LastEntry.LogLevel);
            Assert.Equal(0, LastEntry.EventId.Id);

            var properties = LastEntry.State as IEnumerable<KeyValuePair<string, object>>;
            Assert.NotNull(properties);
            var property = properties.Single();
            Assert.Equal("{OriginalFormat}", property.Key);
            Assert.Equal("message", property.Value);

            Assert.Null(LastEntry.Exception);
            Assert.Equal("message", LastEntry.Text);
        }

        [Fact]
        public void LoggingExceptionTest()
        {
            var exception = new Exception("exception message");
            Logger.LogInformation(exception, "message");

            Assert.Equal("category", LastEntry.Category);
            Assert.Equal(LogLevel.Information, LastEntry.LogLevel);
            Assert.Equal(0, LastEntry.EventId.Id);

            var properties = LastEntry.State as IEnumerable<KeyValuePair<string, object>>;
            Assert.NotNull(properties);
            var property = properties.Single();
            Assert.Equal("{OriginalFormat}", property.Key);
            Assert.Equal("message", property.Value);

            Assert.Equal(exception, LastEntry.Exception);
            Assert.Equal("message", LastEntry.Text);
        }
    }
}
