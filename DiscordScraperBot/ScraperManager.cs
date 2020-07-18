using DiscordScraperBot.Discord;
using DiscordScraperBot.Scrapers;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordScraperBot
{
    public class ScraperManager
    {
        List<Scraper> Scrapers;
        Bot DiscordBot;
        public int Delay { get; set; }
        public DateTime LastScrapeTime { get; private set; }

        const int ScraperDefaultDelay = 1000;
        
        public ScraperManager(Bot bot)
        {
            DiscordBot = bot;
            Scrapers = new List<Scraper>();

            Delay = ScraperDefaultDelay;
        }

        public void AddScraper(Scraper scraper)
        {
            Scrapers.Add(scraper);
        }

        public async Task StartScraping()
        {
            while (true)
            {
                //Console.WriteLine("[+] Scraper running...");
                if (DiscordBot.IsReady)
                {
                    foreach (Scraper scraper in Scrapers)
                    {
                        LastScrapeTime = DateTime.Now;
                        scraper.Scrape();
                        await DiscordBot.SendToChannelAsync(scraper.GetMessages());
                        scraper.ClearMessages();
                    }
                }
                
                Thread.Sleep(Delay);
            }
        }

        public List<Scraper> GetScrapers()
        {
            return Scrapers;
        }

        public int GetTotalCacheSize()
        {
            int cacheSize = 0;
            foreach (Scraper scraper in Scrapers)
            {
                cacheSize += scraper.GetCacheSize();
            }

            return cacheSize;
        }
    }
}
