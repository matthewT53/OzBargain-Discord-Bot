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
            Delay = (Config.bot.scrapeDelay != 0) ? Config.bot.scrapeDelay * 1000 : ScraperDefaultDelay;
            LastClearTime = DateTime.Now;
        }

        public void AddScraper(Scraper scraper)
        {
            Scrapers.Add(scraper);
        }

        public async Task StartScraping()
        {
            TimeSpan TimeUntilCacheClear = new TimeSpan(7, 0, 0, 0);
            while (true)
            {
                if (DiscordBot.IsReady)
                {
                    foreach (Scraper scraper in Scrapers)
                    {
                        scraper.Scrape();

                        LastScrapeTime = DateTime.Now;

                        Console.WriteLine("[+] Size of messages: " + scraper.GetMessages().Count);

                        await DiscordBot.SendToChannelAsync(scraper.GetMessages());
                        scraper.ClearMessages();
                    }

                    DateTime currentTime = DateTime.Now;
                    if (currentTime - LastClearTime > TimeUntilCacheClear)
                    {
                        foreach (Scraper scraper in this.Scrapers)
                        {
                            scraper.ClearCache();
                            Logger.GetInstance().realLogger.Info("Cleared cache for scraper: " + scraper.Name);
                        }

                        LastClearTime = currentTime;
                    }

                    Thread.Sleep(Delay);
                }
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
