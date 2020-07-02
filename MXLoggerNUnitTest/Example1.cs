using Microsoft.Extensions.Logging;
using NUnit.Framework;
using MXLogger;

[assembly: Parallelizable(ParallelScope.Children)]

namespace MXLoggerNUnitTest
{
    public class SimpleTest
    {
        public ILogger Logger;

        public SimpleTest()
        {
            var factory = new LoggerFactory().AddMXLogger(TestContext.WriteLine, LogLevel.Trace);
            Logger = factory.CreateLogger("CategoryName");
        }

        [Test]
        public void Test()
        {
            Logger.LogInformation("message");
        }
    }
}
