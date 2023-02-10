using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;

namespace MXLogger.xUnitTests
{
    public class SimpleExample
    {
        private readonly ILogger Logger;

        public SimpleExample(ITestOutputHelper output)
        {
            ILoggerFactory factory = LoggerFactory
                .Create(builder => builder
                    .AddMXLogger(output.WriteLine)
                    .SetMinimumLevel(LogLevel.Debug));

            Logger = factory.CreateLogger("category");
        }

        [Fact]
        public void Test()
        {
            Logger.LogInformation("message");
            /* ... */
        }
    }
}
