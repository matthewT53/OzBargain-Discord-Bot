using DiscordScraperBot.BotMessages;
using System;
using System.Collections.Generic;

namespace DiscordScraperBot.Scapers
{
    public abstract class Scraper
    {
        List<IBotMessage> Messages;
        Preferences UserPreferences;
        int depth = Int32.MaxValue;
        string Name;

        protected Scraper(Preferences preferences, string name)
        {
            this.Messages = new List<IBotMessage>();
            this.UserPreferences = preferences;
            this.Name = name;
        }

        /***
         * Calling this function will scrape the targeted website for items.
         * These items will be stored in a list that is managed by a concrete 
         * implementation of this interface. 
         * 
         * Every derived class must implement this method.
         */
        public abstract void Scrape();

        public abstract string GetName();

        public void AddMessage(IBotMessage message)
        {
            Messages.Add(message);
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
