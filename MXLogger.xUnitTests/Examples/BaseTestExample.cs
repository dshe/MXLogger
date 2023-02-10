using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;

namespace MXLogger.xUnitTests
{
    public class MyComponent2
    {
        private readonly ILogger Logger;

        public MyComponent2(ILogger logger)
        {
            Logger = logger;
        }

        public void Run()
        {
            Logger.LogInformation("message");
            /* ... */
        }
    }

    public abstract class BaseTest
    {
        protected readonly ILogger<Example> Logger;
        protected MyComponent2 MyComponent2;

        protected BaseTest(ITestOutputHelper output)
        {
            ILoggerFactory factory = LoggerFactory
                .Create(builder => builder
                    .AddMXLogger(output.WriteLine)
                    .SetMinimumLevel(LogLevel.Debug));

            Logger = factory.CreateLogger<Example>();

            ILogger myComponentLogger = factory.CreateLogger<MyComponent2>();
            MyComponent2 = new MyComponent2(myComponentLogger);
        }
    }

    public class Example : BaseTest
    {
        public Example(ITestOutputHelper output) : base(output) { }

        [Fact]
        public void Test()
        {
            Logger.LogInformation("message");

            MyComponent2.Run();
        }
    }

    public class xxx : ILoggerFactory
    {
        public void AddProvider(ILoggerProvider provider)
        {
            throw new System.NotImplementedException();
        }

        public ILogger CreateLogger(string categoryName)
        {
            throw new System.NotImplementedException();
        }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }
    }
}
