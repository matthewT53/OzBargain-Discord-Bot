using DiscordScraperBot.Scapers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace DiscordScraperBot
{
    class ScraperManager
    {
        List<Scraper> _scrapers;
        List<string> _categories;
        List<Item> _items;
        
        public ScraperManager()
        {
            _scrapers = new List<Scraper>();
            _categories = new List<string>();
            _items = new List<Item>();

        }

        private void Initialize()
        {
            // Add all the scrapers that are under the scrapers folder.
            _scrapers.Add(new OzBargainScraper());

            // Load all the user's categories.
            _categories.Add("headphones");
            _categories.Add("games");

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

                Thread.Sleep(1000);
            }
        }
    }
}
