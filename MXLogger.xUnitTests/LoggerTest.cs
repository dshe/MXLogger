using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace MXLogger.xUnitTests
{
    public class LoggerFormatTest
    {
        private readonly Action<string> WriteLine;
        public LoggerFormatTest(ITestOutputHelper output) => WriteLine = output.WriteLine;

        [Fact]
        public void InjectionTest()
        {
            var loggerProvider = new MXLoggerProvider(WriteLine);
            var services = new ServiceCollection().AddLogging(builder => builder.AddProvider(loggerProvider));
            var serviceProvider = services.BuildServiceProvider();

            var factory = serviceProvider.GetService<ILoggerFactory>();
            var logger1 = factory.CreateLogger("category");
            logger1.LogInformation("test");
            Assert.Equal("test", loggerProvider.GetLogEntries().Last().Text);

            var logger2 = serviceProvider.GetService<ILogger<LoggerFormatTest>>();
            logger2.LogInformation("test");
            Assert.Equal("test", loggerProvider.GetLogEntries().Last().Text);
        }

        [Fact]
        public void InjectionWithExtensionTest()
        {
            var services = new ServiceCollection().AddLogging(builder => builder.AddMXLogger(WriteLine));
            var serviceProvider = services.BuildServiceProvider();
            var logger = serviceProvider.GetService<ILogger<LoggerFormatTest>>();
            logger.LogInformation("test");
        }

        [Fact]
        public void LoggingFactoryTest()
        {
            var loggerProvider = new MXLoggerProvider(WriteLine);
            var factory = new LoggerFactory(new[] { loggerProvider });
            var logger = factory.CreateLogger<LoggerFormatTest>();
            logger.LogInformation("anything");
            Assert.Equal("MXLogger.xUnitTests.LoggerFormatTest", loggerProvider.GetLogEntries().Last().Category);
            factory.Dispose();
        }

        [Fact]
        public void LoggingFactoryWithExtensionTest()
        {
            var factory = new LoggerFactory().AddMXLogger(WriteLine);
            var logger = factory.CreateLogger<LoggerFormatTest>();
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
            Assert.Equal("CallerNameTest", loggerProvider.GetLogEntries().Last().Category);
            factory.Dispose();
        }

        [Fact]
        public void TestScopes()
        {
            var loggerProvider = new MXLoggerProvider(WriteLine);
            var factory = new LoggerFactory(new[] { loggerProvider });
            var logger = factory.CreateLogger<LoggerFormatTest>();

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
