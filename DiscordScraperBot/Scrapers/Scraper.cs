using DiscordScraperBot.BotMessages;
using System;
using System.Collections.Generic;

namespace DiscordScraperBot.Scrapers
{
    public abstract class Scraper
    {
        // Store the messages to be posted to the channel
        protected List<IBotMessage> Messages;

        // Stores the messages already seen
        private HashSet<IBotMessage> Cache;

        // The user preferences that are used to filter the items.
        protected Preferences UserPreferences;

        // How many links/pages to scrape.
        public int Depth { get; set; }

        public string Name { get; private set; }

        protected Scraper(Preferences preferences, string name)
        {
            this.Messages = new List<IBotMessage>();
            this.Cache = new HashSet<IBotMessage>();
            this.UserPreferences = preferences;
            this.Name = name;
            this.Depth = (Config.bot.maxDepth != 0) ? Config.bot.maxDepth : Int32.MaxValue;
        }

        /***
         * Calling this function will scrape the targeted website for items.
         * These items will be stored in a list that is managed by a concrete 
         * implementation of this interface. 
         * 
         * Every derived class must implement this method.
         */
        public abstract void Scrape();

        /***
         * Stores a message if it is not already in the cache.
         */
        public void AddMessage(IBotMessage message)
        {
            if (!Cache.Contains(message))
            {
                Messages.Add(message);
            }

            Cache.Add(message);
        }

        /***
         * This function will return a list of items that were scraped from the 
         * source. 
         */
        public List<IBotMessage> GetMessages()
        {
            return Messages;
        }

        /***
         * Clears the list of scraped items.
         */
        public void ClearMessages()
        {
            Messages.Clear();
        }

        /***
         * Clears the cached messages.
         */
        public void ClearCache()
        {
            Cache.Clear();
        }

        /***
         * Returns the number of items in the cache.
         */
        public int GetCacheSize()
        {
            return Cache.Count;
        }
    }
}
