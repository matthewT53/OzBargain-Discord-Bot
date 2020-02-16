using System;
using System.Collections.Generic;
using System.Text;
using DiscordScraperBot;

namespace DiscordScraperBot
{
    /*
     * The purpose of this class is to store the user's shopping preferences 
     * which includes the categories they are after (e.g electronics , clothing, etc)
     * as well as price range.
     */
    public class Preferences
    {
        IStorage _store;

        public Preferences(IStorage storage)
        {
            _store = storage;
            _store.CreatePreferenceTable();
        }

        public bool AddCategory(string category)
        {
            return true;
        }

        public bool AddCategories(List<string> categories)
        {
            return false;
        }

        public bool RemoveCategory(string category)
        {
            return false;
        }

        public bool RemoveCategories(List<string> categories)
        {
            return false;
        }

        public bool AddPriceRange(string category, Tuple<double, double> priceRange)
        {
            return false;
        }

        public bool RemovePriceRange(string category)
        {
            return false;
        }

        public Tuple<double, double> GetPriceRange(string category)
        {
            return null;
        }

        public List<string> GetCategories()
        {
            return null;
        }
    }
}
