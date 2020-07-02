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
                Console.WriteLine("Unable to find config file.");
                bot = new BotConfig();
                WriteConfig(bot);
            }

            else
            {
                Console.WriteLine("Found config file.");
                bot = ReadConfig();

                Console.WriteLine("Token read: " + bot.token);
                Console.WriteLine("Command prefix: " + bot.commandPrefix);
                Console.WriteLine("Channel ID: " + bot.bargainChannelID);
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
        }
    }
}
