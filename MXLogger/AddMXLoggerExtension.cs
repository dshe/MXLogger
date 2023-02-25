using System;
using System.Runtime.CompilerServices;

namespace Microsoft.Extensions.Logging
{
    public static partial class MXLoggerExtensions
    {
        public static ILoggingBuilder AddMXLogger(this ILoggingBuilder builder, Action<string> writeLine)
        {
            var provider = new MXLoggerProvider(writeLine);

            try
            {
                return builder.AddProvider(provider);
            }
            finally
            {
                provider.Dispose();
            }
        }

        [Obsolete("Please use AddMXLogger(ILoggingBuilder,Action<string>) with SetMinimumLevel(ILoggingBuilder,LogLevel) instead.")]
        public static ILoggingBuilder AddMXLogger(this ILoggingBuilder builder, Action<string> writeLine, LogLevel logLevel)
        {
            var provider = new MXLoggerProvider(writeLine, logLevel);

            try
            {
                return builder.AddProvider(provider);
            }
            finally
            {
                provider.Dispose();
            }
        }

        [Obsolete("Please use AddMXLogger(ILoggingBuilder,Action<string>) instead.")]
        public static ILoggerFactory AddMXLogger(this ILoggerFactory factory, Action<string> writeLine, LogLevel logLevel = LogLevel.Trace)
        {
            if (factory is null)
                throw new ArgumentNullException(nameof(factory));

            var provider = new MXLoggerProvider(writeLine, logLevel);

            try
            {
                factory.AddProvider(provider);
                return factory;
            }
            finally
            {
                provider.Dispose();
            }
        }
    }
}
