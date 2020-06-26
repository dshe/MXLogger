using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;
using MXLogger;

namespace MXLoggerTest1
{
    public class SimpleTest
    {
        public readonly ILogger Logger;

        public SimpleTest(ITestOutputHelper output)
        {
            var loggerFactory = new LoggerFactory().AddMXLogger(output.WriteLine);
            Logger = loggerFactory.CreateLogger("CategoryName");
        }

        [Fact]
        public void Test()
        {
            Logger.LogInformation("message");
        }
    }
}
