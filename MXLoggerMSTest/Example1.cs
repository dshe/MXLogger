using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MXLogger;

[assembly: Parallelize(Workers = 0, Scope = ExecutionScope.MethodLevel)]

namespace MXLoggerMSTest
{
    [TestClass]
    public class SimpleTest
    {
        public TestContext? TestContext { get; set; }

        public ILogger Logger;

        public SimpleTest()
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
