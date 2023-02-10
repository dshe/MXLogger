using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace MXLogger.xUnitTests
{
    public class LogLevelTests
    {
        private readonly Action<string> WriteLine;
        public LogLevelTests(ITestOutputHelper output) => WriteLine = output.WriteLine;

        private void TestLogger(ILogger logger)
        {
            logger.LogTrace("test trace");
            logger.LogDebug("test debug");
            logger.LogInformation("test information");
            logger.LogWarning("test warning");
            logger.LogError("test error");
            WriteLine("WriteLine");
            WriteLine("WriteLine");
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
                    .SetMinimumLevel(LogLevel.Information));

            var logger1 = factory.CreateLogger("category");

            TestLogger(logger1);
        }
    }
}
