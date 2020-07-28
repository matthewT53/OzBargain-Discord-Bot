using System;
using Xunit;
using DiscordScraperBot.Discord;
using DiscordScraperBot;

namespace DiscordScraperBot.UnitTests
{
    public class CommandHandlerTests
    {
        public class InitializeCommandHandlerTests
        {
            /*
             * The ScraperManager being equal to null is not detrimental to the 
             * initialization of the InitializeCommandHandler context class.
             */
            [Fact]
            public void TestNullScraperManager()
            {
                Bot bot = new Bot();
                ScraperManager scraper_manager = new ScraperManager(bot);
                Preferences preferences = new Preferences(new MockStorage());
                InitializeCommandHandler init = new InitializeCommandHandler(scraper_manager, bot, preferences);

                Assert.NotNull(init.Client);
                Assert.NotNull(init.Commands);
                Assert.NotNull(init.Services);
            }
        }

        public class CommandHandlerClassTests
        {
            /*
             * The InitializeCommandHandler argument cannot be null because it is used 
             * to initialize the CommandHandler object.
             */
            [Fact]
            public void TestNullInitializeCommandHandler()
            {
                Assert.Throws<ArgumentNullException>(() =>
                {
                    CommandHandler command_handler = new CommandHandler(null);
                });
            }
        }
    }
}
