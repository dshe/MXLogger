using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;

namespace MXLogger.xUnitTests
{ 
    public class InjectionTests
    {
        private readonly Action<string> WriteLine;
        public InjectionTests(ITestOutputHelper output)
        {
            WriteLine = output.WriteLine;
        }

        [Fact]
        public void Test()
        {
            var provider = new MXLoggerProvider(WriteLine);

            var serviceProvider = new ServiceCollection()
                .AddLogging(builder => builder.AddProvider(provider))
                .AddTransient<Component1>()
                .AddTransient<Component2>()
                .BuildServiceProvider();

            // inject ILogger<T>
            Component1 component1 = serviceProvider.GetRequiredService<Component1>()!;
            component1.Log("test");
            Assert.Equal("Info: MXLogger.xUnitTests.Component1\r\ntest\r\n", provider.Format(provider.GetLogEntries().Last()));

            // inject ILoggerFactory
            Component2 component2 = serviceProvider.GetRequiredService<Component2>()!;
            component2.Log("test");
            Assert.Equal("Info: Component2Name\r\ntest\r\n", provider.Format(provider.GetLogEntries().Last()));
        }
    }

    public class Component1
    {
        private readonly ILogger Logger;

        public Component1(ILogger<Component1> logger)
        {
            Logger = logger;
        }

        public void Log(string s)
        {
            Logger.LogInformation(s);
        }
     }

    public class Component2
    {
        private readonly ILogger Logger;

        public Component2(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger("Component2Name");
        }

        public void Log(string s)
        {
            Logger.LogInformation(s);
        }
    }

}
