using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MXLogger.MSTests
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
    public class InjectionExample
    {
        private static Action<string>? WriteLine;

        [ClassInitialize]
        public static void TestFixtureSetup(TestContext context)
        {
            WriteLine = context.WriteLine;
        }

        public MyComponent MyComponent { get; }

        public InjectionExample()
        {
            MyComponent = new ServiceCollection()
                .AddTransient<MyComponent>()
                .AddLogging(builder => builder
                    .AddMXLogger(WriteLine!)
                    .SetMinimumLevel(LogLevel.Trace))
                .BuildServiceProvider()
                .GetRequiredService<MyComponent>();
        }

        [TestMethod]
        public void Test()
        {
            MyComponent.Logger.LogCritical("message");
        }
    }
}
