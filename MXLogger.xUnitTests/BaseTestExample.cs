using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;

namespace MXLogger.xUnitTests
{
    public abstract class BaseTest
    {
        protected readonly ILogger Logger;

        public BaseTest(ITestOutputHelper output)
        {
            Logger = new LoggerFactory()
                .AddMXLogger(output.WriteLine)
                .CreateLogger("Test");
        }
    }

    public class Example : BaseTest
    {
        public Example(ITestOutputHelper output) : base(output) { }

        [Fact]
        public void Test()
        {
            Logger.LogInformation("message");
        }
    }
}
