using System;
using Microsoft.Extensions.Logging;

#nullable enable

namespace MXLogger
{
    internal class MXLogger : ILogger
    {
        private readonly MXLoggerProvider Provider;
        private readonly string Category;
        internal MXLogger(MXLoggerProvider provider, string category)
        {
            Provider = provider ?? throw new ArgumentNullException(nameof(provider));
            Category = category ?? throw new ArgumentNullException(nameof(category)); ;
        }

        IDisposable ILogger.BeginScope<TState>(TState state)
        {
            if (Provider.ScopeProvider == null)
                throw new ArgumentNullException(nameof(Provider.ScopeProvider));
            return Provider.ScopeProvider.Push(state);
        }

        bool ILogger.IsEnabled(LogLevel logLevel) => Convert.ToInt32(logLevel) >= Convert.ToInt32(Provider.LogLevel);

        /// ILogger extension methods call this method
        void ILogger.Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string?> formatter)
        {
            if (((ILogger)this).IsEnabled(logLevel))
            {
                string? text = formatter?.Invoke(state, exception);
                var logEntry = new LogInfo(Category, logLevel, eventId, state, exception, text);
                Provider.Log(logEntry);
            }
        }
    }
}
