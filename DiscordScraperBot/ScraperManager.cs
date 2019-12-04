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
        bool _do_scrape;
        
        public ScraperManager()
        {
            _scrapers = new List<Scraper>();
            _categories = new List<string>();
            _items = new List<Item>();

            _do_scrape = false;
        }

        private void Initialize()
        {
            // Add all the scrapers that are under the scrapers folder.
            _scrapers.Add(new OzBargainScraper());

            // Load all the user's categories.


        }

        public void StartScraping()
        {
            _do_scrape = true;
            while (_do_scrape)
            {
                Console.WriteLine("[+] Scraper running...");
                Thread.Sleep(1000);
            }
        }

        public void StopScraping()
        {
            _do_scrape = false;
        }
    }
}
