using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace WebStore.Logger
{
    public class Log4NetProvider : ILoggerProvider
    {
        private readonly string _ConfigurationFile;
        private readonly ConcurrentDictionary<string, Log4NetLogger> _Loggers = new ConcurrentDictionary<string, Log4NetLogger>();

        public Log4NetProvider(string configurationFile) => _ConfigurationFile = configurationFile;

        public ILogger CreateLogger(string categoryName)
        {
            return _Loggers.GetOrAdd(categoryName, category =>
            {
                var xml = new XmlDocument();
                xml.Load(_ConfigurationFile);
                return new Log4NetLogger(category, xml["log4net"]);
            });
        }

        public void Dispose() => _Loggers.Clear();
    }

    
}
