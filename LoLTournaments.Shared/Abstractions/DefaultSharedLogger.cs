namespace LoLTournaments.Shared.Abstractions
{

    public class DefaultSharedLogger
    {
        private static ISharedLogger serviceLogger;
        public DefaultSharedLogger(ISharedLogger logger)
        {
            serviceLogger = logger;
        }

        public static void Error(object value)
        {
            serviceLogger.Error(value);
        }

        public static void Warning(object value)
        {
            serviceLogger.Warning(value);
        }

        public static void Log(object value)
        {
            serviceLogger.Log(value);
        }
    }

}