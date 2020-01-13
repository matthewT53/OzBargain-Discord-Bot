using DiscordScraperBot.Scapers;
using DiscordScraperBot.Modules;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace DiscordScraperBot
{
    public class ScraperManager
    {
        List<Scraper>   _scrapers;
        List<string>    _categories;
        List<Item>      _items;
        int             _delay;

        const int SCRAPER_DEFAULT_DELAY = 1000;
        
        public ScraperManager()
        {
            _scrapers = new List<Scraper>();
            _categories = new List<string>();
            _items = new List<Item>();

            // Intialize constants here:
            _delay = SCRAPER_DEFAULT_DELAY;
        }

        public void Initialize()
        {
            // Add all the scrapers that are under the scrapers folder.
            _scrapers.Add(new OzBargainScraper());

            // Load all the user's categories.
            _categories.Add("headphones");
            _categories.Add("games");
        }

        public void SetDelay(int delay)
        {
            _delay = delay;
        }

        public int GetDelay()
        {
            return _delay;
        }

        public void StartScraping()
        {
            while (true)
            {
                Console.WriteLine("[+] Scraper running...");

                foreach (Scraper bot in _scrapers)
                {
                    bot.Scrape();
                }

                Console.WriteLine("[+] Delay: " + _delay);
                Thread.Sleep(_delay);
            }
        }
    }
}
