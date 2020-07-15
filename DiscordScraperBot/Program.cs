using DiscordScraperBot.Scrapers;
using DiscordScraperBot.Discord;
using System.Threading.Tasks;
using System.Threading;

namespace DiscordScraperBot
{
    class Program
    {
        static void Main(string[] args)
        {
            Bot bot = new Bot();

            ScraperManager scraperManager = new ScraperManager(bot);
            InitializeScrapers(scraperManager);

            InitializeCommandHandler init = new InitializeCommandHandler(scraperManager, bot);
            Task botTask = bot.StartAsync(init);

            Task scrapeTask = scraperManager.StartScraping();

            // Wait indefinitely for the bot to finish.
            botTask.Wait(-1);
            scrapeTask.Wait(-1);
        }

        static void InitializeScrapers(ScraperManager scraperManager)
        {
            // Load the categories that the user is interested in
            Preferences preferences = new Preferences(new SqliteStorage());

            scraperManager.AddScraper(new OzBargainScraper(preferences));
        }
    }
}
