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
            Logger.LogInformation("XunitTestBaseExample message");
            Write("test!");
        }
    }
}
