using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using MXLogger;

namespace MXLoggerTest
{
    public class LogEntryTest
    {
        private LogInfo LastEntry => LoggerProvider.LogEntries.LastOrDefault();
        private readonly MXLoggerProvider LoggerProvider;
        private readonly ILogger Logger;

        public LogEntryTest(ITestOutputHelper output)
        {
            LoggerProvider = new MXLoggerProvider(output.WriteLine);
            var loggerFactory = new LoggerFactory(new[] { LoggerProvider });
            Logger = loggerFactory.CreateLogger("category");
        }

        [Fact]
        public void LoggingLevelTest()
        {
            Assert.Equal(LogLevel.Trace, LoggerProvider.LogLevel);
            Assert.Null(LastEntry);
            Logger.LogDebug("debug message");
            Logger.LogInformation("information message");
            Logger.LogCritical("critical message");
            Assert.Equal(3, LoggerProvider.LogEntries.Count);
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
