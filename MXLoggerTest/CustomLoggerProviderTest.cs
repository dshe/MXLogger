using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using MXLogger;

namespace MXLoggerTest
{
    public class MyCustomLoggerProvider : MXLoggerProvider
    {
        public MyCustomLoggerProvider(Action<string> writeLine) : base(writeLine) { }
        public override string? Format(LogInfo logInfo) => "custom:" + logInfo.Text;
    }

    public class CustomLoggerProviderTest
    {
        readonly Action<string> WriteLine;
        public CustomLoggerProviderTest(ITestOutputHelper output) => WriteLine = output.WriteLine;

        [Fact]
        public void DependencyInjectionTest()
        {
            var provider = new MyCustomLoggerProvider(WriteLine);
            var services = new ServiceCollection().AddLogging(builder => builder.AddProvider(provider));
            var serviceProvider = services.BuildServiceProvider();
            var factory = serviceProvider.GetService<ILoggerFactory>();
            var logger = serviceProvider.GetService<ILogger<CustomLoggerProviderTest>>();
            logger.LogInformation("test");
            Assert.Equal("custom:test", provider.Format(provider.LogEntries.Last()));
        }

        [Fact]
        public void LoggingFactoryTest()
        {
            var provider = new MyCustomLoggerProvider(WriteLine);
            var factory = new LoggerFactory(new[] { provider });
            var logger = factory.CreateLogger<CustomLoggerProviderTest>();
            logger.LogInformation("test");
            Assert.Equal("custom:test", provider.Format(provider.LogEntries.Last()));
            factory.Dispose();
        }
    }
}
