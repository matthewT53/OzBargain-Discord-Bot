using Xunit;
using DiscordScraperBot;
using System.IO;

namespace DiscordScraperBot.UnitTests
{
    public class ConfigTests
    {
        /*
         * Test that reading from an empty file serializes the BOT config in 
         * our desired way.
         */
        [Fact]
        public static void TestReadEmptyConfig()
        {
            ClearConfig();
            Assert.Null(Config.bot.token);
            Assert.Null(Config.bot.commandPrefix);
        }

        /*
         * Test reading from the config file when either the token is missing or the command prefix 
         * is missing.
         * 
         * This test is meant to detect errors that occur when a developer modifies the way 
         * configuration data is read and written.
         */
        [Fact]
        public static void TestMissingParameters()
        {
            Config.BotConfig bot = new Config.BotConfig();
            bot.token = "";
            bot.commandPrefix = "test_prefix";

            ClearConfig();
            Config.WriteConfig(bot);
            Config.BotConfig botRead = Config.ReadConfig();

            Assert.Equal("", botRead.token);
            Assert.Equal("test_prefix", botRead.commandPrefix);

            bot.token = "test_token";
            bot.commandPrefix = "";

            ClearConfig();
            Config.WriteConfig(bot);
            Config.BotConfig botRead2 = Config.ReadConfig();

            Assert.Equal("test_token", botRead2.token);
            Assert.Equal("", botRead2.commandPrefix);
        }

        /*
         * This test ensures that we can read and write the token and commandPrefix correctly. 
         * 
         * For this test case, both the token and commandPrefix are provided.
         */
        [Fact]
        public static void TestCorrectParameters()
        {
            Config.BotConfig bot = new Config.BotConfig();
            bot.token = "test_token";
            bot.commandPrefix = "test_prefix";

            ClearConfig();
            Config.WriteConfig(bot);
            Config.BotConfig botRead = Config.ReadConfig();

            Assert.Equal("test_token", bot.token);
            Assert.Equal("test_prefix", bot.commandPrefix);
        }

        /*
         * This function is a private helper function that clears the configuration file used by the 
         */
        private static void ClearConfig()
        {
            File.WriteAllText(Config.ConfigPath, "");   
        }
    }
}
