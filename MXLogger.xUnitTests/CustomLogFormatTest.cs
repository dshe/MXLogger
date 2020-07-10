using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;

namespace MXLogger.xUnitTests
{
    public class MyCustomLoggerProvider : MXLoggerProvider
    {
        public MyCustomLoggerProvider(Action<string> writeLine) : base(writeLine) { }
        public override string? Format(LogInfo logInfo)
        {
            // create custom format here
            return "custom: " + logInfo.Text;
        }
    }

    public class CustomLogFormatTest
    {
        readonly Action<string> WriteLine;
        public CustomLogFormatTest(ITestOutputHelper output)
        {
            WriteLine = output.WriteLine;
        }

        [Fact]
        public void LoggingFactoryTest()
        {
            var provider = new MyCustomLoggerProvider(WriteLine);
            var factory = new LoggerFactory(new[] { provider });
            var logger = factory.CreateLogger<CustomLogFormatTest>();
            
            logger.LogInformation("test");

            Assert.Equal("custom: test", provider.Format(provider.GetLogEntries().Last()));
            factory.Dispose();
        }
    }
}
