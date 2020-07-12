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
        List<Scraper> Scrapers;
        Bot DiscordBot;
        int Delay;

        const int SCRAPER_DEFAULT_DELAY = 1000;
        
        public ScraperManager(Bot bot)
        {
            DiscordBot = bot;
            Scrapers = new List<Scraper>();

            // Intialize constants here:
            Delay = SCRAPER_DEFAULT_DELAY;
        }

        public void AddScraper(Scraper scraper)
        {
            Scrapers.Add(scraper);
        }

        public async Task StartScraping()
        {
            while (true)
            {
                Console.WriteLine("[+] Scraper running...");
                foreach (Scraper scraper in Scrapers)
                {
                    scraper.Scrape();
                    await DiscordBot.SendToChannelAsync(scraper.GetMessages());
                    scraper.ClearMessages();
                }

                Console.WriteLine("[+] Delay: " + Delay);
                Thread.Sleep(Delay);
            }
        }

        public List<Scraper> GetScrapers()
        {
            return Scrapers;
        }

        public void SetDelay(int delay)
        {
            Delay = delay;
        }

        public int GetDelay()
        {
            return Delay;
        }
    }
}
