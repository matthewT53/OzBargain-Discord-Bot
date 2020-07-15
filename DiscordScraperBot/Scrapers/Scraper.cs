using DiscordScraperBot.BotMessages;
using System;
using System.Collections.Generic;

namespace DiscordScraperBot.Scrapers
{
    public abstract class Scraper
    {
        // Store the messages to be posted to the channel
        List<IBotMessage> Messages;

        // Stores the messages already seen
        HashSet<IBotMessage> Cache;

        // The user preferences that are used to filter the items.
        Preferences UserPreferences;

        // How many links/pages to scrape.
        int depth = Int32.MaxValue;

        string Name;

        protected Scraper(Preferences preferences, string name)
        {
            this.Messages = new List<IBotMessage>();
            this.UserPreferences = preferences;
            this.Name = name;
            this.Cache = new HashSet<IBotMessage>();
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
         * Returns the name of the scraper.
         */
        public string GetName()
        {
            return Name;
        }

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
         * Sets how many pages to scrape.
         */
        public void SetDepth(int newDepth)
        {
            depth = newDepth;
        }

        /***
         * Returns the level of scraping this scraper will perform.
         */
        public int GetDepth()
        {
            return depth;
        }
    }
}
