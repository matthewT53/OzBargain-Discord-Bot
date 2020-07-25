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
        public DateTime LastClearTime { get; private set; }

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
                if (DiscordBot.IsReady)
                {
                    foreach (Scraper scraper in Scrapers)
                    {
                        DateTime currentTime = DateTime.Now;
                        TimeSpan TimeUntilCacheClear = new TimeSpan(7, 0, 0, 0);

                        if (currentTime - LastClearTime > TimeUntilCacheClear)
                        {
                            scraper.ClearCache();
                            LastClearTime = currentTime;

                            Logger.GetInstance().realLogger.Info("Cleared cache for scraper: " + scraper.Name);
                        }

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
