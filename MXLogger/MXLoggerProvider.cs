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
        private readonly ConcurrentDictionary<string, MXLogger> loggerCache = new ConcurrentDictionary<string, MXLogger>();

        public MXLoggerProvider(Action<string> writeLine)
        {
            WriteLine = writeLine;
            LogLevel = LogLevel.Trace;
        }
        public MXLoggerProvider(Action<string> writeLine, LogLevel logLevel)
        {
            WriteLine = writeLine;
            LogLevel = logLevel;
        }

        public ILogger CreateLogger(string categoryName) =>
            loggerCache.GetOrAdd(categoryName, category => new MXLogger(this, category));

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

        private readonly List<MXLogInfo> LogEntries = new List<MXLogInfo>();
        public IList<MXLogInfo> GetLogEntries()
        {
            lock (LogEntries)
            {
                return new List<MXLogInfo>(LogEntries);
            }
        }

        // called by XUnitLogger.Log(...)
        internal void Log(MXLogInfo logEntry)
        {
            lock (LogEntries)
            {
                LogEntries.Add(logEntry);
            }

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

        public virtual string? Format(MXLogInfo logInfo)
        {
            if (logInfo is null)
                throw new ArgumentNullException(nameof(logInfo));

            StringBuilder sb = new StringBuilder();

            int indent = 0;
            IReadOnlyList<object?> scopes = GetScopes(logInfo.State);
            if (scopes.Any())
            {
                indent = scopes.Count * 4;
                sb.Append(' ', indent);
                object? lastScope = scopes[scopes.Count - 1];
                if (lastScope is string str && !string.IsNullOrWhiteSpace(str))
                    sb.Append(str + "  ");
            }

            sb.Append(logInfo.LogLevel.ToShortString());
            sb.Append(": ");
            sb.Append(logInfo.Category);

            if (logInfo.EventId.Id != 0)
                sb.Append($"  [{logInfo.EventId.Id}]:{logInfo.EventId.Name}");
            sb.AppendLine();

            if (!string.IsNullOrEmpty(logInfo.Text))
            {
                sb.Append(' ', indent);
                sb.AppendLine(logInfo.Text);
            }

            if (logInfo.Exception != null)
            {
                sb.Append(' ', indent);
                sb.AppendLine(logInfo.Exception.ToString());
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
}
