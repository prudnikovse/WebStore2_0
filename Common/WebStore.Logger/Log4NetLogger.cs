
using log4net;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Xml;

namespace WebStore.Logger
{
    public class Log4NetLogger : ILogger
    {
        private readonly ILog _Log;

        public Log4NetLogger(string categoryName, XmlElement xmlConfig)
        {
            var loggerRepository = LogManager.CreateRepository(
                Assembly.GetEntryAssembly(),
                typeof(log4net.Repository.Hierarchy.Hierarchy));

            _Log = LogManager.GetLogger(loggerRepository.Name, categoryName);
            log4net.Config.XmlConfigurator.Configure(loggerRepository, xmlConfig);
        }

        public IDisposable BeginScope<TState>(TState state) => null;

        public bool IsEnabled(LogLevel logLevel)
        {
            switch(logLevel)
            {
                case LogLevel.Trace:
                case LogLevel.Debug:
                    return _Log.IsDebugEnabled;

                case LogLevel.Information:
                    return _Log.IsInfoEnabled;
                case LogLevel.Warning:
                    return _Log.IsWarnEnabled;
                case LogLevel.Error:
                    return _Log.IsErrorEnabled;
                case LogLevel.Critical:
                    return _Log.IsFatalEnabled;
                case LogLevel.None:
                    return false;

                default: throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null);
            }
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (formatter == null)
                throw new ArgumentNullException(nameof(formatter));

            if (!IsEnabled(logLevel))
                return;

            var logMessage = formatter(state, exception);

            if (string.IsNullOrEmpty(logMessage) && exception == null)
                return;

            switch (logLevel)
            {
                case LogLevel.Trace:
                case LogLevel.Debug:
                    _Log.Debug(logMessage);
                    break;

                case LogLevel.Information:
                    _Log.Info(logMessage);
                    break;
                case LogLevel.Warning:
                    _Log.Warn(logMessage);
                    break;
                case LogLevel.Error:
                    _Log.Error(logMessage ?? $"{exception.Message} {exception.StackTrace}");
                    break;
                case LogLevel.Critical:
                    _Log.Fatal(logMessage ?? $"{exception.Message} {exception.StackTrace}");
                    break;
                case LogLevel.None:
                    return;

                default: throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null);
            }
        }
    }
}
