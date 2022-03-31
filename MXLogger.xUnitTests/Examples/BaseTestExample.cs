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
            Logger.LogInformation("Hello World!");
        }
    }

    public abstract class BaseTest
    {
        protected readonly ILogger<Example> Logger;
        protected MyComponent2 MyComponent2;

        public BaseTest(ITestOutputHelper output)
        {
            ILoggerFactory factory = new LoggerFactory()
               .AddMXLogger(output.WriteLine, LogLevel.Debug);

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
}
