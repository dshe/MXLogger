using System;
using Microsoft.Extensions.Logging;

namespace MXLogger
{
    public class LogInfo
    {
        public string Category { get; }
        public LogLevel LogLevel { get; }
        public EventId EventId { get; }
        public object? State { get; }
        public Exception? Exception { get; }
        public string? Text { get; }

        internal LogInfo(string category, LogLevel logLevel, EventId eventId, object? state, Exception? exception, string? text)
        {
            Category = category;
            LogLevel = logLevel;
            EventId = eventId;
            State = state;
            Exception = exception;
            Text = text;
        }
    }
}
