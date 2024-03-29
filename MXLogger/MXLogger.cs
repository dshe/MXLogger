﻿using System;

namespace Microsoft.Extensions.Logging
{
    internal sealed class MXLogger : ILogger
    {
        private readonly MXLoggerProvider Provider;
        private readonly string Category;
        internal MXLogger(MXLoggerProvider provider, string category)
        {
            Provider = provider;
            Category = category;
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            if (Provider.ScopeProvider is null)
                throw new InvalidOperationException(nameof(Provider.ScopeProvider));

            return Provider.ScopeProvider.Push(state);
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            if (logLevel == LogLevel.None)
                return false;

            return logLevel >= Provider.LogLevel;
        }

        // ILogger extension methods call this method
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel))
                return;

            string? text = formatter?.Invoke(state, exception);

            var logEntry = new MXLogInfo(Category, logLevel, eventId, state, exception, text);

            Provider.Log(logEntry);
        }
    }
}
