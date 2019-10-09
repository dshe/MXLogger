using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

#nullable enable

namespace MXLogger
{
    public class MXLoggerProvider : ILoggerProvider, ISupportExternalScope
    {
        public LogLevel LogLevel { get; }
        private readonly Action<string> WriteLine;
        public MXLoggerProvider(Action<string> writeLine, LogLevel logLevel = LogLevel.Trace)
        {
            WriteLine = writeLine ?? throw new ArgumentNullException(nameof(writeLine));
            LogLevel = logLevel;
        }

        private readonly ConcurrentDictionary<string, MXLogger> loggers = new ConcurrentDictionary<string, MXLogger>();
        ILogger ILoggerProvider.CreateLogger(string Category) => loggers.GetOrAdd(Category, category => new MXLogger(this, category));

        void ISupportExternalScope.SetScopeProvider(IExternalScopeProvider scopeProvider) => ScopeProvider = scopeProvider;
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

        public IList<LogInfo> LogEntries { get; } = new List<LogInfo>();

        // called by XUnitLogger.Log(...)
        internal void Log(LogInfo logEntry)
        {
            LogEntries.Add(logEntry);
            var str = Format(logEntry);
            if (str == null)
                return;
            try
            {
                WriteLine(str);
            }
            catch (Exception)
            {
            }
        }

        public virtual string? Format(LogInfo logInfo)
        {
            if (logInfo == null)
                throw new ArgumentNullException(nameof(logInfo));

            var sb = new StringBuilder();

            var scopes = GetScopes(logInfo.State);
            if (scopes.Any())
            {
                sb.Append(' ', scopes.Count * 5);
                var lastScope = scopes.LastOrDefault();
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
                sb.Append($"\n{logInfo.Exception.ToString()}");

            return sb.ToString();
        }

        void IDisposable.Dispose() { }
    }
}
