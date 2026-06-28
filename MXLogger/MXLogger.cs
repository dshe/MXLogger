using System;
namespace Microsoft.Extensions.Logging;

internal sealed class MXLogger : ILogger
{
    private readonly MXLoggerProvider _provider;
    private readonly string _category;
    internal MXLogger(MXLoggerProvider provider, string category)
    {
        _provider = provider;
        _category = category;
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        if (_provider.ScopeProvider is null)
            throw new InvalidOperationException(nameof(_provider.ScopeProvider));

        return _provider.ScopeProvider.Push(state);
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        if (logLevel == LogLevel.None)
            return false;

        return logLevel >= _provider.LogLevel;
    }

    // ILogger extension methods call this method
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
            return;

        string? text = formatter?.Invoke(state, exception);

        MXLogInfo logEntry = new(_category, logLevel, eventId, state, exception, text);

        _provider.Log(logEntry);
    }
}
