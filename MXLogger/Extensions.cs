using System;
using System.Runtime.CompilerServices;

namespace Microsoft.Extensions.Logging
{
    public static class MXLoggerExtensions
    {
        public static ILoggingBuilder AddMXLogger(this ILoggingBuilder builder, Action<string> writeLine, LogLevel logLevel = LogLevel.Trace)
        {
            var provider = new MXLoggerProvider(writeLine, logLevel);

            try
            {
                return builder.AddProvider(provider);
            }
            finally
            {
                provider.Dispose();
            }
        }

        [Obsolete("Please use AddMXLogger(this ILoggingBuilder builder ...) instead.")]
        public static ILoggerFactory AddMXLogger(this ILoggerFactory factory, Action<string> writeLine, LogLevel logLevel = LogLevel.Trace)
        {
            if (factory is null)
                throw new ArgumentNullException(nameof(factory));

            var provider = new MXLoggerProvider(writeLine, logLevel);

            try
            {
                factory.AddProvider(provider);
                return factory;
            }
            finally
            {
                provider.Dispose();
            }
        }

        public static ILogger CreateLogger(this ILoggerFactory factory, [CallerMemberName] string callerName = "")
        {
            if (factory is null)
                throw new ArgumentNullException(nameof(factory));

            return factory.CreateLogger(callerName);
        }

        public static string ToShortString(this LogLevel level)
        {
            return level switch // Note: switch expression will not compile on Appveyor
            {
                LogLevel.Trace =>        "Trace",
                LogLevel.Debug =>        "Debug",
                LogLevel.Information =>  "Info",
                LogLevel.Warning =>      "Warn",
                LogLevel.Error =>        "Error",
                LogLevel.Critical =>     "Crit",
                LogLevel.None =>         "None",
                _ => throw new InvalidOperationException("Invalid LogLevel."),
            };
        }
    }
}
