using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using MXLogger;

namespace MXLoggerTest
{
    public class LoggerTest0
    {
        private readonly Action<string> WriteLine;
        public LoggerTest0(ITestOutputHelper output) => WriteLine = output.WriteLine;

        [Fact]
        public void InjectionTest()
        {
            var loggerProvider = new MXLoggerProvider(WriteLine);
            var services = new ServiceCollection().AddLogging(builder => builder.AddProvider(loggerProvider));
            var serviceProvider = services.BuildServiceProvider();

            var factory = serviceProvider.GetService<ILoggerFactory>();
            var logger1 = factory.CreateLogger("category");
            logger1.LogInformation("test");
            Assert.Equal("test", loggerProvider.LogEntries.Last().Text);

            var logger2 = serviceProvider.GetService<ILogger<LoggerTest>>();
            logger2.LogInformation("test");
            Assert.Equal("test", loggerProvider.LogEntries.Last().Text);
        }

        [Fact]
        public void InjectionWithExtensionTest()
        {
            var services = new ServiceCollection().AddLogging(builder => builder.AddMXLogger(WriteLine));
            var serviceProvider = services.BuildServiceProvider();
            var logger = serviceProvider.GetService<ILogger<LoggerTest>>();
            logger.LogInformation("test");
        }

        [Fact]
        public void LoggingFactoryTest()
        {
            var loggerProvider = new MXLoggerProvider(WriteLine);
            var factory = new LoggerFactory(new[] { loggerProvider });
            var logger = factory.CreateLogger<LoggerTest>();
            logger.LogInformation("anything");
            Assert.Equal("MXLoggerTest.LoggerTest", loggerProvider.LogEntries.Last().Category);
            factory.Dispose();
        }

        [Fact]
        public void LoggingFactoryWithExtensionTest()
        {
            var factory = new LoggerFactory().AddMXLogger(WriteLine);
            var logger = factory.CreateLogger<LoggerTest>();
            logger.LogInformation("anything");
            factory.Dispose();
        }

        [Fact]
        public void CallerNameTest()
        {
            var loggerProvider = new MXLoggerProvider(WriteLine);
            var factory = new LoggerFactory(new[] { loggerProvider });
            var logger = factory.CreateLogger();
            logger.LogInformation("anything");
            Assert.Equal("CallerNameTest", loggerProvider.LogEntries.Last().Category);
            factory.Dispose();
        }

        [Fact]
        public void TestScopes()
        {
            var loggerProvider = new MXLoggerProvider(WriteLine);
            var factory = new LoggerFactory(new[] { loggerProvider });
            var logger = factory.CreateLogger<LoggerTest>();

            logger.LogError("outsideLoop");
            using (logger.BeginScope("scope1"))
            {
                logger.LogError("loop1");
                using (logger.BeginScope("scope2"))
                {
                    logger.LogError("loop2");
                    using (logger.BeginScope("scope3"))
                    {
                        logger.LogError("loop3");
                    }
                }
            }
            factory.Dispose();
        }
    }
}
