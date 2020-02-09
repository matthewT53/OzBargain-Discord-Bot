using DiscordScraperBot.Discord;
using DiscordScraperBot.Scapers;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordScraperBot
{
    public class ScraperManager
    {
        List<Scraper> _scrapers;
        int _delay;
        Bot _bot;

        const int SCRAPER_DEFAULT_DELAY = 1000;
        
        public ScraperManager(Bot bot)
        {
            _bot = bot;
            _scrapers = new List<Scraper>();

            // Intialize constants here:
            _delay = SCRAPER_DEFAULT_DELAY;
        }

        public void AddScraper(Scraper scraper)
        {
            _scrapers.Add(scraper);
        }

        public void StartScraping()
        {
            while (true)
            {
                Console.WriteLine("[+] Scraper running...");

                foreach (Scraper scraper in _scrapers)
                {
                    scraper.Scrape();

                    // There is no result so we do not need to await.
                    _bot.SendMessagesAsync(scraper.GetMessages());
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
