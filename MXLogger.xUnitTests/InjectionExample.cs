using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace MXLogger.xUnitTests
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
        public readonly MyComponent MyComponent;

        public InjectionExample(ITestOutputHelper output)
        {
            MyComponent = new ServiceCollection()
                .AddTransient<MyComponent>()
                .AddLogging(builder => builder.AddMXLogger(output.WriteLine, LogLevel.Trace))
                .BuildServiceProvider()
                .GetService<MyComponent>()!;
        }

        [Fact]
        public void Test()
        {
            MyComponent.Logger.LogCritical("message");
        }
    }
}
