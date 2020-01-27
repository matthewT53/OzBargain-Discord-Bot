using DiscordScraperBot.Scapers;
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

            InitializeCommandHandler init = new InitializeCommandHandler(scraperManager);
            Task botTask = bot.StartAsync(init);

            Thread thread = new Thread(scraperManager.StartScrapingAsync);
            thread.Start();
            thread.Join();

            // Wait indefinitely for the bot to finish.
            botTask.Wait(-1);
        }

        static void InitializeScrapers(ScraperManager scraperManager)
        {
            ulong productsChannelId = 0;
            scraperManager.AddScraper(new OzBargainScraper(productsChannelId));
        }
    }
}
