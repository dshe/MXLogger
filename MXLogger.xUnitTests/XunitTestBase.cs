using Microsoft.Extensions.Logging;
using System;

namespace Xunit.Abstractions;

public abstract class XunitTestBase
{
    private readonly ITestOutputHelper Output;
    protected void Write(string format, params object[] args) =>
            Output.WriteLine(string.Format(format, args) + Environment.NewLine);
    protected readonly ILoggerFactory LogFactory;
    protected readonly ILogger Logger;

    protected XunitTestBase(ITestOutputHelper output, LogLevel logLevel = LogLevel.Debug, string name = "Test")
    {
        Output = output;
        LogFactory = LoggerFactory
            .Create(builder => builder
                .AddMXLogger(output.WriteLine)
                .SetMinimumLevel(logLevel));
        Logger = LogFactory.CreateLogger(name);
    }
}
