using LoLTournaments.Shared.Abstractions;
using Microsoft.Extensions.Logging;

namespace LoLTournaments.Application.Services
{

    public class AppLoggerService : ISharedLogger
    {
        private readonly ILogger<AppLoggerService> logger;
        public AppLoggerService(ILogger<AppLoggerService> logger)
        {
            this.logger = logger;
        }

        public void Error(object value)
        {
            logger.Log(LogLevel.Error, value?.ToString());
        }

        public void Warning(object value)
        {
            logger.Log(LogLevel.Warning, value?.ToString());
        }

        public void Log(object value)
        {
            logger.Log(LogLevel.Debug, value?.ToString());
        }
    }

}