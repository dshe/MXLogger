using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MXLogger;

namespace MXLoggerMSTest
{
    public class MyComponent
    {
        public readonly ILogger Logger;

        public MyComponent(ILogger<MyComponent> logger)
        {
            Logger = logger;
        }
    }

    [TestClass]
    public class DependencyInjectionTest
    {
        public TestContext? TestContext { get; set; }

        public MyComponent? MyComponent { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            MyComponent = new ServiceCollection()
                .AddTransient<MyComponent>()
                .AddLogging(builder => builder.AddMXLogger(TestContext!.WriteLine, LogLevel.Trace))
                .BuildServiceProvider()
                .GetService<MyComponent>();
        }

        [TestMethod]
        public void Test()
        {
            MyComponent!.Logger.LogCritical("message");
        }
    }
}
