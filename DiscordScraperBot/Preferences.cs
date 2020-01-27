using System;
using System.Collections.Generic;
using System.Text;
using DiscordScraperBot.Storage;

namespace DiscordScraperBot
{
    /*
     * The purpose of this class is to store the user's shopping preferences 
     * which includes the categories they are after (e.g electronics , clothing, etc)
     * as well as price range.
     */
    public class Preferences
    {
        IStorage _db;
        public Preferences(IStorage storage)
        {
            _db = storage;
        }

        public void AddCategory(string category)
        {

        }

        public void RemoveCategory(string category)
        {

        }

        public void AddPriceRange(Tuple<double, double> priceRange)
        {
            
        }

        public void RemovePriceRange(Tuple<double, double> priceRange)
        {
             
        }

        public List<string> GetCategories()
        {
            return null;
        }
    }
}
