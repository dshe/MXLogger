using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;
using MXLogger;

namespace MXLoggerTest2
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
        public readonly MyComponent MyComponent;

        public DependencyInjectionTest(ITestOutputHelper output)
        {
            MyComponent = new ServiceCollection()
                .AddTransient<MyComponent>()
                .AddLogging(builder => builder.AddMXLogger(output.WriteLine))
                .BuildServiceProvider()
                .GetService<MyComponent>();
        }

        [Fact]
        public void Test()
        {
            MyComponent.Logger.LogCritical("message");
        }
    }
}
