using Contracts.Generic;
using NLog;


namespace Services.Generic
{
    public class LoggerManager : ILoggerManager
    {
        private static ILogger logger;
        public LoggerManager()
        {
            logger = LogManager.GetCurrentClassLogger();
        }
        public void logDebug(string message)
        {
            logger.Debug(message);
        }

        public void logError(string message)
        {
            logger.Error(message);
        }

        public void logInformation(string message)
        {
            logger.Info(message);
        }

        public void logWarning(string message)
        {
            logger.Warn(message);
        }
    }
}
