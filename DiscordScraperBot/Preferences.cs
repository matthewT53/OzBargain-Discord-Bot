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

        public void AddCategory(string category)
        {

        }

        public void RemoveCategory(string category)
        {

        }

        public void AddPriceRange(string category, Tuple<double, double> priceRange)
        {
            
        }

        public void RemovePriceRange(string category, Tuple<double, double> priceRange)
        {
             
        }

        public List<string> GetCategories()
        {
            return null;
        }
    }
}
