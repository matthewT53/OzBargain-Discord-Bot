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
        IStorage _storage;

        public Preferences(IStorage storage)
        {
            _storage = storage;
            _storage.CreatePreferenceTable();
        }

        public bool AddCategory(string category)
        {
            UserPreference preference = new UserPreference(category);
            return _storage.InsertUserPreference(preference);
        }

        public bool AddCategories(List<string> categories)
        {
            List<UserPreference> preferences = new List<UserPreference>();
            foreach (string category in categories)
            {
                preferences.Add(new UserPreference(category));
            }

            return _storage.InsertUserPreferences(preferences);
        }

        public bool RemoveCategory(string category)
        {
            UserPreference preference = new UserPreference(category);
            return _storage.RemoveCategory(preference);
        }

        public bool RemoveCategories(List<string> categories)
        {
            List<UserPreference> preferences = new List<UserPreference>();
            foreach (string category : categories)
            {
                preferences.Add(category);
            }

            return _storage.RemoveCategories(preferences);
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
