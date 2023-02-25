using System;

namespace Microsoft.Extensions.Logging
{
    public static partial class MXLoggerExtensions
    {
        public static ILoggerFactory ToLoggerFactory(this ILogger logger) => new MXFactoryLogger(logger);
    }

    internal sealed class MXFactoryLogger : ILoggerFactory
    {
        private readonly ILogger Logger;
        internal MXFactoryLogger(ILogger logger) => Logger = logger;
        public void AddProvider(ILoggerProvider provider) => throw new NotImplementedException();
        public ILogger CreateLogger(string categoryName) => Logger;
        public void Dispose() { }
    }
}
