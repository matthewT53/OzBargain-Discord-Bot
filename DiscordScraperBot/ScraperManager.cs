using DiscordScraperBot.Discord;
using DiscordScraperBot.Scapers;
using System;
using System.Collections.Generic;
using System.Threading;

namespace DiscordScraperBot
{
    public class ScraperManager
    {
        List<IScraper> _scrapers;
        int _delay;
        Endpoint _endpoint;

        const int SCRAPER_DEFAULT_DELAY = 1000;
        
        public ScraperManager()
        {
            _scrapers = new List<IScraper>();

            // Intialize constants here:
            _delay = SCRAPER_DEFAULT_DELAY;
        }

        public void AddScraper(IScraper scraper)
        {
            _scrapers.Add(scraper);
        }

        public void StartScraping()
        {
            while (true)
            {
                Console.WriteLine("[+] Scraper running...");

                foreach (IScraper bot in _scrapers)
                {
                    bot.Scrape();
                }

                Console.WriteLine("[+] Delay: " + _delay);
                Thread.Sleep(_delay);
            }
        }

        public void SetDelay(int delay)
        {
            _delay = delay;
        }

        public int GetDelay()
        {
            return _delay;
        }
    }
}
