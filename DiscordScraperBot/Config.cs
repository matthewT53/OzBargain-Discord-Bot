using Newtonsoft.Json;
using System;
using System.IO;

namespace DiscordScraperBot
{
    public class Config
    {
        private const string configFolder = "Resources";
        private const string configFile = "config.json";

        public const string ConfigPath = configFolder + "/" + configFile;
        public static BotConfig bot;

        static Config()
        {
            if (!Directory.Exists(configFolder))
            {
                Directory.CreateDirectory(configFolder);
            }

            if (!File.Exists(ConfigPath))
            {
                bot = new BotConfig();
                WriteConfig(bot);
            }

            else
            {
                bot = ReadConfig();

                Console.WriteLine("Command prefix: " + bot.commandPrefix);
                Console.WriteLine("Channel ID: " + bot.bargainChannelID);
                Console.WriteLine("scrapeDelay: " + bot.scrapeDelay);
                Console.WriteLine("postDelay: " + bot.postDelay);
                Console.WriteLine("maxDepth: " + bot.maxDepth);
                Console.WriteLine("logfile: " + bot.logFile);
            }
        }

        public static void WriteConfig(BotConfig bot)
        {
            string json = JsonConvert.SerializeObject(bot, Formatting.Indented);
            File.WriteAllText(ConfigPath, json);
        }

        public static BotConfig ReadConfig()
        {
            string json = File.ReadAllText(ConfigPath);
            BotConfig bot = JsonConvert.DeserializeObject<BotConfig>(json);
            return bot;        
        }

        public struct BotConfig
        {
            public string token;
            public string commandPrefix;

            // These are the ID's of the channels for each specific type of scraper.
            public ulong bargainChannelID;
            
            // Common settings for the bot and its scrapers
            public int scrapeDelay;
            public int postDelay;
            public int maxDepth;

            public string logFile;
        }
    }
}
