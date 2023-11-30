using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;

namespace MXLogger.xUnitTests
{
    public class XunitTestBaseExample : XunitTestBase
    {
        public XunitTestBaseExample(ITestOutputHelper output) : base(output) { }

        [Fact]
        public void Test()
        {
            Logger.LogInformation("XunitTestBaseExample message1");
            Logger.LogInformation("XunitTestBaseExample message2");
            Write("test3");
            Write("test4");
            Logger.LogInformation("XunitTestBaseExample message5");
            Write("test6");
        }
    }
}
