using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace MXLogger.xUnitTests
{
    public class MyComponent1
    {
        private readonly ILogger Logger;

        public MyComponent1(ILogger<MyComponent1> logger)
        {
            Logger = logger;
        }

        public void Run()
        {
            Logger.LogCritical("Test!");
        }
    }

    public class InjectionExample
    {
        private readonly MyComponent1 MyComponent1;

        public InjectionExample(ITestOutputHelper output)
        {
            MyComponent1 = new ServiceCollection()
                .AddTransient<MyComponent1>()
                .AddLogging(builder => builder
                    .AddMXLogger(output.WriteLine)
                    .SetMinimumLevel(LogLevel.Debug))
                .BuildServiceProvider()
                .GetRequiredService<MyComponent1>()!;
        }

        [Fact]
        public void Test()
        {
            MyComponent1.Run();
        }
    }
}
