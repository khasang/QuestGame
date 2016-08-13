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
                .MinimumLevel.Verbose()     // ставим минимальный уровень в Verbose для теста, по умолчанию стоит Information
                .WriteTo.ColoredConsole()
                .WriteTo.RollingFile(@"E:\Temp\Logs\Log-{Date}.txt") // а также пишем лог файл, разбивая его по дате
                .WriteTo.Seq("http://localhost:7805")
                .WriteTo.LiterateConsole(outputTemplate: "{Timestamp:HH:mm:ss} [{EventType:x8} {Level}] {Message}{NewLine}{Exception}")
                // есть возможность писать Verbose уровень в текстовый файл, а например, Error в Windows Event Logs
                .CreateLogger();
        }

        public void Debug(string message, params object[] property)
        {
            logger.Debug(message, @property);
        }

        public void Error(string message, params object[] property)
        {
            logger.Error(message, @property);
        }

        public void Warning(string message, params object[] property)
        {
            logger.Warning(message, @property);
        }

        public void Information(string message, params object[] property)
        {
            logger.Information(message, @property);
        }

        public static ILoggerService Create()
        {
            return new LoggerService();
        }
    }
}
