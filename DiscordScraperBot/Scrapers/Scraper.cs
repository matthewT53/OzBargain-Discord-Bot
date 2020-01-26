using DiscordScraperBot.BotMessages;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordScraperBot.Scapers
{
    public abstract class Scraper
    {
        ulong _channelId;
        List<IBotMessage> _messages;

        protected Scraper(ulong channelId)
        {
            _channelId = channelId;
            _messages = new List<IBotMessage>();
        }

        /*
         * Calling this function will scrape the targeted website for items.
         * These items will be stored in a list that is managed by a concrete 
         * implementation of this interface. 
         * 
         * Every derived class must implement this method.
         */
        public abstract void Scrape();

        /*
         * Associates this scraper with a specific channel. It makes sense to associate a scraper with a channel. 
         * At this stage a scraper should NOT have the ability to send messages to different channels.
         */
        public ulong GetChannelId()
        {
            return _channelId;
        }

        /*
         * This function will return a list of items that were scraped from the 
         * source. 
         */
        public List<IBotMessage> GetMessages()
        {
            return _messages;
        }

        /*
         * Clears the list of scraped items.
         */
        public void ClearMessages()
        {
            _messages.Clear();
        }
    }
}
