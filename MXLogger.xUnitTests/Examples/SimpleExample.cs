using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;

namespace MXLogger.xUnitTests
{
    public class SimpleExample
    {
        public readonly ILogger Logger;

        public SimpleExample(ITestOutputHelper output)
        {
            Logger = new LoggerFactory()
                .AddMXLogger(output.WriteLine, LogLevel.Trace)
                .CreateLogger("CategoryName");
           }

        [Fact]
        public void Test()
        {
            Logger.LogInformation("some message");
        }
    }
}
