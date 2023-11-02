using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;

namespace MXLogger.xUnitTests
{
    public class SimpleExample
    {
        protected readonly ILogger Logger;

        public SimpleExample(ITestOutputHelper output)
        {
            Logger = LoggerFactory
                .Create(builder => builder
                    .AddMXLogger(output.WriteLine)
                    .SetMinimumLevel(LogLevel.Debug))
               .CreateLogger("category");
        }

        [Fact]
        public void Test()
        {
            Logger.LogInformation("message");
            /* ... */
        }
    }
}
