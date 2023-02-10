using System;
using System.Runtime.CompilerServices;

namespace Microsoft.Extensions.Logging
{
    public static class ReducedLoggerExtension
    {
        public static ILogger Reduce(this ILogger logger) =>
            new ReducedLogger(logger);
    }
   
    internal sealed class ReducedLogger : ILogger
    {
        private readonly ILogger Logger;
        internal ReducedLogger(ILogger logger) => Logger = logger;

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull =>
            Logger.BeginScope(state);

        public bool IsEnabled(LogLevel logLevel)
        {
            return Logger.IsEnabled(logLevel);
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (logLevel is LogLevel.Trace || logLevel is LogLevel.None)
                return;
            logLevel--; // reduce
            Logger.Log(logLevel, eventId, state, exception, formatter);
        }
    }
    
}
