using DiscordScraperBot.BotMessages;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordScraperBot.Scapers
{
    public interface IScraper
    {
        /*
         * Calling this function will scrape the targeted website for items.
         * These items will be stored in a list that is managed by a concrete 
         * implementation of this interface. 
         */
        public void Scrape();

        /*
         * Associates this scraper with a specific channel.
         */
        public void SetChannelID(ulong channelID);
        public ulong GetChannelID();

        /*
         * This function will return a list of items that were scraped from the 
         * source. 
         */
        public List<IBotMessage> GetItems();

        /*
         * Clears the list of scraped items.
         */
        public void ClearItems();
    }
}
