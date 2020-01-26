using DiscordScraperBot.Scapers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace DiscordScraperBot
{
    class Program
    {
        static ScraperManager _scraperManager;
        static InitializeCommandHandler _init;

        static void Main(string[] args)
        {
            Bot b = new Bot();
            _scraperManager = new ScraperManager();

            _init = new InitializeCommandHandler(_scraperManager);
            b.StartAsync(_init).GetAwaiter().GetResult();

            Thread thread = new Thread(_scraperManager.StartScraping);
            thread.Start();
        }

        static void InitializeScrapers()
        {
            _scraperManager.AddScraper(new OzBargainScraper());
        }
    }
}
