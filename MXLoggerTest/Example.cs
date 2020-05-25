using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;
using MXLogger;

namespace MXLoggerTest
{
    public class LoggerTest
    {
        protected readonly ILoggerFactory LoggerFactory;
        protected readonly ILogger Logger;

        public LoggerTest(ITestOutputHelper output)
        {
            LoggerFactory = new LoggerFactory().AddMXLogger(output.WriteLine);
            Logger = LoggerFactory.CreateLogger<LoggerTest>();
        }

        [Fact]
        public void Test1()
        {
            Logger.LogInformation("message");
        }

        [Fact]
        public void Test2()
        {
            var logger = LoggerFactory.CreateLogger("SomeLoggerCategoryName");
            logger.LogDebug("message");
        }
    }

    public class LoggerDependencyInjectionTest
    {
        protected readonly IServiceProvider ServiceProvider;

        public LoggerDependencyInjectionTest(ITestOutputHelper output)
        {
            IServiceCollection services = new ServiceCollection();
            services.AddLogging(builder => builder.AddMXLogger(output.WriteLine));
            ServiceProvider = services.BuildServiceProvider();
        }

        [Fact]
        public void Test()
        {
            var logger = ServiceProvider.GetService<ILogger<LoggerTest>>();
            logger.LogCritical("message");
        }
    }
}
