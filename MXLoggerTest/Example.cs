using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;
using MXLogger;
using System.Threading;

namespace MXLoggerTest
{
    public class LoggerTest
    {
        private readonly Action<string> WriteLine;
        public LoggerTest(ITestOutputHelper output) => WriteLine = output.WriteLine;

        [Fact]
        public void FactoryTest()
        {
            ILoggerFactory factory = new LoggerFactory().AddMXLogger(WriteLine);
            ILogger logger = factory.CreateLogger<LoggerTest>();
            logger.LogCritical("message");
        }

        [Fact]
        public void InjectionTest()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddLogging(builder => builder.AddMXLogger(WriteLine));
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            ILogger logger = serviceProvider.GetService<ILogger<LoggerTest>>();
            logger.LogInformation("message");
        }

        [Fact]
        public void TimestampTest()
        {
            var provider = new MXLoggerProvider(LogLevel.Trace);
            var factory = new LoggerFactory();
            factory.AddProvider(provider);
            ILogger logger = factory.CreateLogger<LoggerTest>();

            logger.LogInformation("message1");
            logger.LogWarning("message2");
            Thread.Sleep(1000);
            logger.LogCritical("message3");

            provider.WriteTo(WriteLine);
        }
    }
}
