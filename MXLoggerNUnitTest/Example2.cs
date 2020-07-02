using Microsoft.Extensions.Logging;
using NUnit.Framework;
using MXLogger;
using Microsoft.Extensions.DependencyInjection;

namespace MXLoggerNUnitTest
{
    public class MyComponent
    {
        public readonly ILogger Logger;

        public MyComponent(ILogger<MyComponent> logger)
        {
            Logger = logger;
        }
    }

    public class DependencyInjectionTest
    {
        public MyComponent MyComponent;

        public DependencyInjectionTest()
        {
            MyComponent = new ServiceCollection()
                .AddTransient<MyComponent>()
                .AddLogging(builder => builder.AddMXLogger(TestContext.WriteLine, LogLevel.Trace))
                .BuildServiceProvider()
                .GetService<MyComponent>();
        }

        [Test]
        public void Test()
        {
            MyComponent.Logger.LogCritical("message");
        }
    }
}
