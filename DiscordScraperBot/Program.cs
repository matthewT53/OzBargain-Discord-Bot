using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace DiscordScraperBot
{
    class Program
    {
        static void Main(string[] args)
        {
            Bot b = new Bot();
            ScraperManager scraper_manager = new ScraperManager();

            InitializeCommandHandler init = new InitializeCommandHandler(scraper_manager);
            b.StartAsync(init).GetAwaiter().GetResult();

            Thread thread = new Thread(scraper_manager.StartScraping);
            thread.Start();
        }
    }
}
