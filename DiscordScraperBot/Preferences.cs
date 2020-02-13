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
        }

        public bool AddCategory(string category)
        {

        }

        public bool RemoveCategory(string category)
        {

        }

        public bool AddPriceRange(string category, Tuple<double, double> priceRange)
        {
            
        }

        public bool RemovePriceRange(string category)
        {
             
        }

        public Tuple<double, double> GetPriceRange(string category)
        {
            
        }

        public List<string> GetCategories()
        {
            return null;
        }
    }
}
