using Newtonsoft.Json;
using System.IO;

namespace DiscordScraperBot
{
    class Config
    {
        private const string configFolder = "Resources";
        private const string configFile = "config.json";

        public static BotConfig bot;

        static Config()
        {
            if (!Directory.Exists(configFolder))
            {
                Directory.CreateDirectory(configFolder);
            }

            string path = configFolder + "/" + configFile;
            if (!File.Exists(path))
            {
                bot = new BotConfig();
                string json = JsonConvert.SerializeObject(bot, Formatting.Indented);
                File.WriteAllText(path, json);
            }

            else
            {
                string json = File.ReadAllText(path);
                bot = JsonConvert.DeserializeObject<BotConfig>(json);
            }
        }

        public struct BotConfig
        {
            public string token;
            public string cmdPrefix;
        }
    }
}
