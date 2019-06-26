using Microsoft.Extensions.Logging;
using System;
using System.Runtime.CompilerServices;

namespace MXLogger
{
    public static class Extensions
    {
        public static ILoggingBuilder AddXLogger(this ILoggingBuilder builder, Action<string> writeLine, LogLevel logLevel = LogLevel.Trace)
        {
            var provider = new MXLoggerProvider(writeLine, logLevel);
            builder.AddProvider(provider);
            return builder;
        }

        public static ILoggerFactory AddXLogger(this ILoggerFactory factory, Action<string> writeLine, LogLevel logLevel = LogLevel.Trace)
        {
            var provider = new MXLoggerProvider(writeLine, logLevel);
            factory.AddProvider(provider);
            return factory;
        }
        public static ILogger CreateLogger(this ILoggerFactory factory, [CallerMemberName] string callerName = "") =>
            factory.CreateLogger(callerName);

        public static string ToShortString(this LogLevel level)
        {
            return level switch
            {
                LogLevel.Trace => "Trace",
                LogLevel.Debug => "Debug",
                LogLevel.Information => "Info",
                LogLevel.Warning => "Warn",
                LogLevel.Error => "Error",
                LogLevel.Critical => "Crit",
                LogLevel.None => "None",
                _ => throw new Exception("Invalid LogLevel."),
            };
        }
    }
}
