using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[assembly: Parallelize(Workers = 0, Scope = ExecutionScope.MethodLevel)]

namespace MXLogger.MSTests
{
    [TestClass]
    public class SimpleExample
    {
        public TestContext? TestContext { get; set; }

        public ILogger Logger;

        public SimpleExample()
        {
            var factory = new LoggerFactory().AddMXLogger(s => TestContext!.WriteLine(s), LogLevel.Trace);
            Logger = factory.CreateLogger("CategoryName");
        }

        [TestMethod]
        public void Test()
        {
            Logger.LogInformation("message");
        }
    }


}
