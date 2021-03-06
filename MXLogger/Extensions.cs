﻿using System;
using System.Runtime.CompilerServices;

namespace Microsoft.Extensions.Logging
{
    public static class Extensions
    {
        public static ILoggingBuilder AddMXLogger(this ILoggingBuilder builder, Action<string> writeLine, LogLevel logLevel = LogLevel.Trace)
        {
            var provider = new MXLoggerProvider(writeLine, logLevel);
            return builder.AddProvider(provider);
        }

        public static ILoggerFactory AddMXLogger(this ILoggerFactory factory, Action<string> writeLine, LogLevel logLevel = LogLevel.Trace)
        {
            var provider = new MXLoggerProvider(writeLine, logLevel);
            factory.AddProvider(provider);
            return factory;
        }

        public static ILogger CreateLogger(this ILoggerFactory factory, [CallerMemberName] string callerName = "") =>
            factory.CreateLogger(callerName);

        public static string ToShortString(this LogLevel level)
        {
            switch (level) // Note: switch expression will not compile on Appveyor
            {
                case LogLevel.Trace:       return "Trace";
                case LogLevel.Debug:       return "Debug";
                case LogLevel.Information: return "Info";
                case LogLevel.Warning:     return "Warn";
                case LogLevel.Error:       return "Error";
                case LogLevel.Critical:    return "Crit";
                case LogLevel.None:        return "None";
            }
            throw new Exception("Invalid LogLevel.");
        }
    }
}
