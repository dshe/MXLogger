using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;

namespace Microsoft.Extensions.Logging
{
    public class MXLoggerProvider : ILoggerProvider, ISupportExternalScope
    {
        public LogLevel LogLevel { get; }
        private readonly Action<string> WriteLine;
        private readonly ConcurrentDictionary<string, MXLogger> loggers = new ConcurrentDictionary<string, MXLogger>();

        public MXLoggerProvider(Action<string> writeLine, LogLevel logLevel = LogLevel.Trace) =>
            (WriteLine, LogLevel) = (writeLine, logLevel);

        public ILogger CreateLogger(string categoryName) =>
            loggers.GetOrAdd(categoryName, category => new MXLogger(this, category));

        public void SetScopeProvider(IExternalScopeProvider scopeProvider) => ScopeProvider = scopeProvider;
        internal IExternalScopeProvider? ScopeProvider;

        public IReadOnlyList<object?> GetScopes(object? state)
        {
            var scopes =  new List<object?>();
            ScopeProvider?.ForEachScope((value, loggingProps) =>
            {
                if (value is IEnumerable<KeyValuePair<string, object>> properties)
                    scopes.Add(properties.ToDictionary(i => i.Key, i => i.Value));
                else
                    scopes.Add(value);
            }, state);
            return scopes.AsReadOnly();
        }

        private ConcurrentBag<LogInfo> LogEntries { get; } = new ConcurrentBag<LogInfo>();
        public IList<LogInfo> GetLogEntries() => LogEntries.ToArray().OrderBy(x => x.DateTime).ToList();

        public void Write(string text)
        {
            var logEntry = new LogInfo("", LogLevel.Critical, 0, null, null, text);
            LogEntries.Add(logEntry);
        }

        // called by XUnitLogger.Log(...)
        internal void Log(LogInfo logEntry)
        {
            LogEntries.Add(logEntry);
            string? str = Format(logEntry);
            if (str is null)
                return;
            try
            {
                WriteLine(str);
            }
            catch (Exception)
            {
                ;
            }
        }

        public virtual string? Format(LogInfo logInfo)
        {
            if (logInfo == null)
                throw new ArgumentNullException(nameof(logInfo));

            StringBuilder sb = new StringBuilder();

            IReadOnlyList<object?> scopes = GetScopes(logInfo.State);
            if (scopes.Any())
            {
                sb.Append(' ', scopes.Count * 5);
                object? lastScope = scopes[scopes.Count - 1];
                if (lastScope is string str && !string.IsNullOrWhiteSpace(str))
                    sb.Append(str + "\t  ");
            }

            sb.Append($"{logInfo.LogLevel.ToShortString()}\t  ");
            sb.Append($"{logInfo.Category}\t  ");

            if (logInfo.EventId.Id != 0)
                sb.Append($"[{logInfo.EventId.Id}]:{logInfo.EventId.Name}\t  ");

            if (!string.IsNullOrEmpty(logInfo.Text))
                sb.Append($"{logInfo.Text}\t  ");

            if (logInfo.Exception != null)
                sb.Append($"\n{logInfo.Exception}");

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
}
