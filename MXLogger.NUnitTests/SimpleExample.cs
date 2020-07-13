using Microsoft.Extensions.Logging;
using NUnit.Framework;

[assembly: Parallelizable(ParallelScope.Children)]

namespace MXLogger.NUnitTests
{
    public class SimpleExample
    {
        public ILogger Logger;

        public SimpleExample()
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
