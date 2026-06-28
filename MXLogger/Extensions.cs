using System;
using System.Runtime.CompilerServices;
namespace Microsoft.Extensions.Logging;

public static partial class MXLoggerExtensions
{
    extension(ILoggingBuilder builder)
    {
        public ILoggingBuilder AddMXLogger(Action<string> writeLine, MXLogFormatType loggerFormatType = MXLogFormatType.Standard)
        {
            MXLoggerProvider provider = new(writeLine, loggerFormatType);
            try
            {
                return builder.AddProvider(provider);
            }
            finally
            {
                provider.Dispose();
            }
        }
    }

    extension(ILoggerFactory factory)
    {
        public ILogger CreateLoggerFromCallerMemberName([CallerMemberName] string callerName = "")
        {
            if (factory is null)
                throw new ArgumentNullException(nameof(factory));

            return factory.CreateLogger(callerName);
        }
    }

    extension(LogLevel level)
    {
        public string ToShortString()
        {
            return level switch
            {
                LogLevel.Trace       => "Trace",
                LogLevel.Debug       => "Debug",
                LogLevel.Information => "Info",
                LogLevel.Warning     => "Warn",
                LogLevel.Error       => "Error",
                LogLevel.Critical    => "Crit",
                LogLevel.None        => "None",
                _ => throw new InvalidOperationException("Invalid LogLevel."),
            };
        }
    }
}
