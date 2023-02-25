using Microsoft.Extensions.Logging;
using NUnit.Framework;

[assembly: Parallelizable(ParallelScope.Children)]

namespace MXLogger.NUnitTests
{
    public class SimpleExample
    {
        private ILogger Logger;

        public SimpleExample()
        {
            ILoggerFactory factory = LoggerFactory
                .Create(builder => builder
                    .AddMXLogger(TestContext.WriteLine)
                    .SetMinimumLevel(LogLevel.Trace));

            Logger = factory.CreateLogger("CategoryName");
        }

        [Test]
        public void Test()
        {
            Logger.LogInformation("message");
        }
    }
}
