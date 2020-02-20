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
            UserPreference preference = _preferences.Find((pref) =>
            {
                return pref._category == category;
            });

            return _storage.DeleteUserPreference(preference);
        }

        public bool RemoveCategories(List<string> categories)
        {
            List<UserPreference> preferencesToRemove = new List<UserPreference>();
            foreach (string category in categories)
            {
                UserPreference preference = _preferences.Find((pref) =>
                {
                    return pref._category == category;
                });

                preferencesToRemove.Add(preference);
            }

            return _storage.DeleteUserPreferences(preferencesToRemove);
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
