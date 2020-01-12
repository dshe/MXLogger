using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace MXLogger
{
    public class MXLoggerProvider : ILoggerProvider, ISupportExternalScope
    {
        private readonly long Time = Stopwatch.GetTimestamp();
        public LogLevel LogLevel { get; }
        private readonly Action<string>? WriteLine;
        public MXLoggerProvider(LogLevel logLevel = LogLevel.Trace) => LogLevel = logLevel;
        public MXLoggerProvider(Action<string> writeLine, LogLevel logLevel = LogLevel.Trace)
        {
            WriteLine = writeLine;
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
            if (WriteLine == null)
                return;
            var str = Format(logEntry);
            if (str == null)
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

        public void WriteTo(Action<string> writeLine)
        {
            var str = LogEntries
                .OrderBy(e => e.Time)
                .Select(e => Format(e))
                .Aggregate(new StringBuilder(),(current, next) => current.Append(current.Length == 0 ? "" : "\r\n").Append(next))
                .ToString();
            writeLine(str);
        }

        public virtual string? Format(LogInfo logInfo)
        {
            if (logInfo == null)
                throw new ArgumentNullException(nameof(logInfo));

            var sb = new StringBuilder();

            if (logInfo.Time != 0 && WriteLine == null)
                sb.AppendFormat("{0:####0.000} ", TimeSpan.FromTicks(logInfo.Time - Time).TotalMilliseconds);

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
