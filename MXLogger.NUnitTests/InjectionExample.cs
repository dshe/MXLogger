using Microsoft.Extensions.Logging;
using NUnit.Framework;
using Microsoft.Extensions.DependencyInjection;

namespace MXLogger.NUnitTests
{
    public class MyComponent
    {
        public readonly ILogger Logger;

        public MyComponent(ILogger<MyComponent> logger)
        {
            Logger = logger;
        }
    }

    public class InjectionExample
    {
        public MyComponent MyComponent;

        public InjectionExample()
        {
            MyComponent = new ServiceCollection()
                .AddTransient<MyComponent>()
                .AddLogging(builder => builder.AddMXLogger(TestContext.WriteLine, LogLevel.Trace))
                .BuildServiceProvider()
                .GetService<MyComponent>()!;
        }

        [Test]
        public void Test()
        {
            MyComponent.Logger.LogCritical("message");
        }
    }
}
