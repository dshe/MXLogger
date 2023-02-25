using System;
using System.Runtime.CompilerServices;

namespace Microsoft.Extensions.Logging
{
    public static partial class MXLoggerExtensions
    {
        public static ILogger CreateLoggerFromCallerMemberName(this ILoggerFactory factory, [CallerMemberName] string callerName = "")
        {
            if (factory is null)
                throw new ArgumentNullException(nameof(factory));

            return factory.CreateLogger(callerName);
        }

        internal static string ToShortString(this LogLevel level)
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
