using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;
using MXLogger;

namespace MXLoggerXunitTest0
{
    public class SimpleTest
    {
        public readonly ILogger Logger;

        public SimpleTest(ITestOutputHelper output)
        {
            ILoggerFactory loggerFactory = new LoggerFactory().AddMXLogger(output.WriteLine, LogLevel.Trace);
            Logger = loggerFactory.CreateLogger("CategoryName");
        }

        [Fact]
        public void Test()
        {
            Logger.LogInformation("message");
        }
    }
}
