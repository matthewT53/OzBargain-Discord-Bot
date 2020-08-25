using LLibrary;
using System;

namespace DiscordScraperBot
{
    class Logger
    {
        private static Logger instance = null;
        public L realLogger = null;

        private Logger()
        {
            realLogger = new L(
                // True to use UTC time rather than local time.
                // Defaults to false.
                useUtcTime: true,

                // If other than null it sets to delete any file in the log folder that is older than the time set.
                // Defaults to null.
                deleteOldFiles: TimeSpan.FromDays(10),

                // Format string to use when calling DateTime.Format.
                // Defaults to "yyyy-MM-dd HH:mm:ss".
                dateTimeFormat: "dd MMM HH:mm:ss",

                // Directory where to create the log files.
                // Defaults to null, which creates a local "logs" directory.
                directory: string.IsNullOrEmpty(Config.bot.logFile) ? "C:\\Logs\\" : Config.bot.logFile
            );
        }

        public static Logger GetInstance()
        {
            if (instance == null)
            {
                instance = new Logger();
            }

            return instance;
        }
    }
}
