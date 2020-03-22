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

        // Cache layer for user preferences
        List<UserPreference> _preferences;

        public Preferences(IStorage storage)
        {
            _storage = storage;
            _storage.CreatePreferenceTable();

            // Load the existing preferences from the database.
            _preferences = _storage.GetUserPreferences();
        }

        public bool AddCategory(string category)
        {
            UserPreference preference = new UserPreference(category);
            _preferences.Add(preference);
            return _storage.InsertUserPreference(preference);
        }

        public bool AddCategory(string category, Tuple<double, double> priceRange)
        {
            UserPreference preference = new UserPreference(category, priceRange.Item1, priceRange.Item2);
            _preferences.Add(preference);
            return _storage.InsertUserPreference(preference);
        }

        public bool AddCategories(List<string> categories)
        {
            foreach (string category in categories)
            {
                _preferences.Add(new UserPreference(category));
            }

            return _storage.InsertUserPreferences(_preferences);
        }

        public bool RemoveCategory(string category)
        {
            UserPreference preference = FindUserPreferenceCache(category);
            return _storage.DeleteUserPreference(preference);
        }

        public bool RemoveCategories(List<string> categories)
        {
            List<UserPreference> preferencesToRemove = new List<UserPreference>();
            foreach (string category in categories)
            {
                UserPreference preference = FindUserPreferenceCache(category);
                preferencesToRemove.Add(preference);
            }

            return _storage.DeleteUserPreferences(preferencesToRemove);
        }

        public bool AddPriceRange(string category, Tuple<double, double> priceRange)
        {
            UserPreference preference = FindUserPreferenceCache(category);
            preference._minPrice = priceRange.Item1;
            preference._maxPrice = priceRange.Item2;

            return _storage.UpdateUserPreference(preference);
        }

        public bool RemovePriceRange(string category)
        {
            UserPreference preference = FindUserPreferenceCache(category);
            preference._minPrice = 0.0;
            preference._maxPrice = 0.0;

            return _storage.UpdateUserPreference(preference);
        }

        public Tuple<double, double> GetPriceRange(string category)
        {
            UserPreference preference = FindUserPreferenceCache(category);
            Tuple<double, double> priceRange = new Tuple<double, double>(preference._minPrice, preference._maxPrice);
            return priceRange;
        }

        /***
         * Returns the names of all the categories stored in the list of user preferences 
         * from the cache layer.
         */
        public List<string> GetCategories()
        {
            List<string> categories = new List<string>();

            foreach (UserPreference preference in _preferences)
            {
                categories.Add(preference._category);
            }

            return categories;
        }

        /***
         * Returns a user preference from the cache if found otherwise null is returned 
         */
        private UserPreference FindUserPreferenceCache(string category)
        {
            UserPreference preference = _preferences.Find((pref) =>
            {
                return pref._category == category;
            });

            return preference;
        }
    }
}
