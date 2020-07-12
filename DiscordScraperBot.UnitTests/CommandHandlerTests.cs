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
                InitializeCommandHandler init = new InitializeCommandHandler(scraper_manager, bot);

                Assert.NotNull(init._client);
                Assert.NotNull(init._commands);
                Assert.NotNull(init._services);
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
