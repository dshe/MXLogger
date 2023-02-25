using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;

namespace MXLogger.xUnitTests
{
    public class LogLevelTests
    {
        private readonly Action<string> WriteLine;
        public LogLevelTests(ITestOutputHelper output) => WriteLine = output.WriteLine;

        private static void TestLogger(ILogger logger)
        {
            //WriteLine("WriteLine");
            //WriteLine("WriteLine");
            logger.LogTrace("test trace");
            logger.LogDebug("test debug");
            logger.LogInformation("test information");
            logger.LogWarning("test warning");
            logger.LogError("test error");
            logger.LogCritical("test critical");
            logger.Log(LogLevel.None, "test none");
        }

        [Fact]
        public void Test1()
        {
            List<string> strings = new();

            var factory = LoggerFactory
                .Create(builder => builder
                    .AddMXLogger(WriteLine)
                    .SetMinimumLevel(LogLevel.Debug));

            var logger1 = factory.CreateLogger("category");

            TestLogger(logger1);
        }
    }
}
