using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
namespace Microsoft.Extensions.Logging;

public class MXLoggerProvider : ILoggerProvider, ISupportExternalScope
{
    public LogLevel LogLevel { get; } = LogLevel.Trace;
    private readonly MXLogFormatType _formatType;
    private readonly Action<string> _writeLine;
    private readonly ConcurrentDictionary<string, MXLogger> _loggerCache = new();

    public MXLoggerProvider(Action<string> writeLine, MXLogFormatType formatType = MXLogFormatType.Standard)
    {
        _writeLine = writeLine;
        _formatType = formatType;
    }

    public ILogger CreateLogger(string categoryName) =>
        _loggerCache.GetOrAdd(categoryName, category => new MXLogger(this, category));

    public void SetScopeProvider(IExternalScopeProvider scopeProvider) => ScopeProvider = scopeProvider;
    internal IExternalScopeProvider? ScopeProvider;

    public IReadOnlyList<object?> GetScopes(object? state)
    {
        List<object?> scopes = [];

        ScopeProvider?.ForEachScope((value, loggingProps) =>
        {
            if (value is IEnumerable<KeyValuePair<string, object>> properties)
                scopes.Add(properties.ToDictionary(i => i.Key, i => i.Value));
            else
                scopes.Add(value);
        }, state);

        return scopes.AsReadOnly();
    }

    public bool GetScopeInfo(MXLogInfo logInfo, out int number, out string name)
    {
        IReadOnlyList<object?> scopes = GetScopes(logInfo.State);
        number = scopes.Count;
        name = "";
        if (number > 0)
        {
            object? lastScope = scopes[number - 1];
            if (lastScope is string str && !string.IsNullOrWhiteSpace(str))
            {
                name = str;
                return true;
            }
        }
        return false;
    }

    private readonly List<MXLogInfo> _logEntries = [];
    public IList<MXLogInfo> GetLogEntries()
    {
        lock (_logEntries)
        {
            return [.. _logEntries];
        }
    }

    // called by XUnitLogger.Log(...)
    internal void Log(MXLogInfo logEntry)
    {
        lock (_logEntries)
        {
            _logEntries.Add(logEntry);
        }

        string? str = Format(logEntry);
        try
        {
            if (str != null)
                _writeLine(str);
        }
        catch (Exception)
        {
            ;
        }
    }

    public virtual string? Format(MXLogInfo logInfo)
    {
        if (logInfo is null)
            throw new ArgumentNullException(nameof(logInfo));

        if (_formatType == MXLogFormatType.SingleLine)
            return SingleLineFormat(logInfo);

        return StandardFormat(logInfo);
    }

    public string StandardFormat(MXLogInfo logInfo)
    {
        StringBuilder sb = new();

        int indent = 0;
        if (GetScopeInfo(logInfo, out int scopes, out string name))
        {
            indent = scopes * 4;
            sb.Append(' ', indent);
            sb.Append(name);
            sb.Append(' ', 2);
        }

        sb.Append((logInfo.LogLevel.ToShortString() + ":").PadRight(7));
        sb.Append(logInfo.Category);

        if (logInfo.EventId.Id != 0)
            sb.Append($"  [{logInfo.EventId.Id}]:{logInfo.EventId.Name}");
        sb.AppendLine();
        sb.Append(' ', indent);

        if (!string.IsNullOrEmpty(logInfo.Text))
            sb.Append(logInfo.Text);

        if (logInfo.Exception != null)
        {
            sb.AppendLine();
            sb.Append(logInfo.Exception.ToString());
        }
        sb.AppendLine();

        return sb.ToString();
    }

    public string SingleLineFormat(MXLogInfo logInfo)
    {
        StringBuilder sb = new();

        if (GetScopeInfo(logInfo, out int scopes, out string name))
        {
            sb.Append(' ', scopes * 2);
            sb.Append(name);
            sb.Append(' ', 2);
        }

        sb.Append(logInfo.LogLevel.ToString()[0]);
        sb.Append("> ");
        sb.Append(logInfo.Category.Substring(logInfo.Category.LastIndexOf('.') + 1));
        sb.Append(": ");

        if (!string.IsNullOrEmpty(logInfo.Text))
            sb.Append(logInfo.Text);

        if (logInfo.Exception != null)
        {
            sb.AppendLine();
            sb.Append(logInfo.Exception.ToString());
        }

        return sb.ToString();
    }

    protected virtual void Dispose(bool disposing) { }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

}
