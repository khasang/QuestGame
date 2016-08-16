using QuestGame.Common.Interfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestGame.Common
{
    public class LoggerService : ILoggerService
    {
        ILogger logger;

        public LoggerService()
        {
            logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.RollingFile(@"D:\Temp\Logs\Log-{Date}.txt")
                .CreateLogger();
        }

        public void Debug(string message, params object[] property)
        {
            logger.Debug(message, property);
        }

        public void Error(string message, params object[] property)
        {
            logger.Error(message, property);
        }

        public void Information(string message, params object[] property)
        {
            logger.Information(message, property);
        }

        public void Warning(string message, params object[] property)
        {
            logger.Warning(message, property);
        }
    }
}
