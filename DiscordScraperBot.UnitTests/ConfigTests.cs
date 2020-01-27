using Xunit;
using DiscordScraperBot;
using System.IO;
using System;

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
            DeleteConfig();
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
        public static void TestMissingToken()
        {
            Config.BotConfig bot = new Config.BotConfig();
            bot.token = "";
            bot.commandPrefix = "test_prefix";
            bot.productsChannelId = 1;
            bot.newsChannelId = 2;
            bot.jobsChannelId = 3;

            DeleteConfig();
            Config.WriteConfig(bot);
            Config.BotConfig botRead = Config.ReadConfig();

            Assert.Equal("", botRead.token);
            Assert.Equal("test_prefix", botRead.commandPrefix);
            Assert.Equal<ulong>(1, botRead.productsChannelId);
            Assert.Equal<ulong>(2, botRead.newsChannelId);
            Assert.Equal<ulong>(3, botRead.jobsChannelId);
        }

        [Fact]
        public static void TestMissingCommandPrefix()
        {
            Config.BotConfig bot = new Config.BotConfig();
            bot.token = "test_token";
            bot.commandPrefix = "";
            bot.productsChannelId = 6;
            bot.newsChannelId = 5;
            bot.jobsChannelId = 4;

            DeleteConfig();
            Config.WriteConfig(bot);
            Config.BotConfig botRead = Config.ReadConfig();

            Assert.Equal("test_token", botRead.token);
            Assert.Equal("", botRead.commandPrefix);
            Assert.Equal<ulong>(6, botRead.productsChannelId);
            Assert.Equal<ulong>(5, botRead.newsChannelId);
            Assert.Equal<ulong>(4, botRead.jobsChannelId);
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

            DeleteConfig();
            Config.WriteConfig(bot);
            Config.BotConfig botRead = Config.ReadConfig();

            Assert.Equal("test_token", botRead.token);
            Assert.Equal("test_prefix", botRead.commandPrefix);
        }

        /*
         * Test reading from an invalid config file throws an appropriate exception.
         */
        [Fact]
        public static void TestInvalidConfigFile()
        {
            File.WriteAllText(Config.ConfigPath, "garbage_json_format");
           
            Assert.Throws<Exception>(() =>
            {
                try
                {
                    Config.BotConfig bot = Config.ReadConfig();
                }

                catch (Exception ex)
                {
                    throw new Exception("[Exception] Invalid configuration file!");
                }
            });
        }

        /*
         * This function is a private helper function that clears the 
         * configuration file used by the static Config class.
         */
        private static void DeleteConfig()
        {
            try
            {
                File.Delete(Config.ConfigPath);
            }

            catch (DirectoryNotFoundException ex)
            {
                System.Console.WriteLine("[Exception] " + ex.Message);
            }
        }
    }
}
