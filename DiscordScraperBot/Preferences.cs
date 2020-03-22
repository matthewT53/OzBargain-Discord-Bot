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
            if (storage == null)
            {
                throw new ArgumentNullException("storage cannot be null.");
            }
            _storage = storage;
            _storage.CreatePreferenceTable();

            // Load the existing preferences from the database.
            _preferences = _storage.GetUserPreferences();
        }

        /***
         * Creates a UserPreference object for a category and then inserts into the database.
         * Returns true if the UserPreference was successfully added into the storage and false otherwise.
         */
        public bool AddCategory(string category)
        {
            if (category == null)
            {
                throw new ArgumentNullException("category cannot be null.");
            }

            UserPreference preference = new UserPreference(category);
            _preferences.Add(preference);
            return _storage.InsertUserPreference(preference);
        }

        /***
         * Creates a UserPreference object with a price range for a category and then inserts into the database.
         * Returns true if the UserPreference was successfully added into the storage and false otherwise.
         */
        public bool AddCategory(string category, Tuple<double, double> priceRange)
        {
            UserPreference preference = new UserPreference(category, priceRange.Item1, priceRange.Item2);
            _preferences.Add(preference);
            return _storage.InsertUserPreference(preference);
        }

        /***
         * Creates a UserPreference object for each category in categories list and then inserts them into the database.
         * Returns true if all the categories were successfully added into the storage and false otherwise.
         */
        public bool AddCategories(List<string> categories)
        {
            foreach (string category in categories)
            {
                _preferences.Add(new UserPreference(category));
            }

            return _storage.InsertUserPreferences(_preferences);
        }

        /***
         * Removes a UserPreference from the storage corresponding to the given category.
         * Returns true if the removal was successful from the underlying storage and false otherwise.
         */
        public bool RemoveCategory(string category)
        {
            UserPreference preference = FindUserPreferenceCache(category);
            return _storage.DeleteUserPreference(preference);
        }

        /***
         * Removes all the UserPreference object corresponding to all categories inside the categories list. 
         * Returns true if successful and false otherwise.
         */
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

        /***
         * Associates a price range with a certain category.
         * Returns true if successful and false otherwise.
         * 
         * Throws:
         * 1. ArgumentNullException - If priceRange is null.
         * 2. UserPreferenceNotFoundException - If category is not found.
         */
        public bool AddPriceRange(string category, Tuple<double, double> priceRange)
        {
            UserPreference preference = FindUserPreferenceCache(category);

            if (priceRange == null)
            {
                throw new ArgumentNullException("priceRange cannot be null.");
            }

            preference._minPrice = priceRange.Item1;
            preference._maxPrice = priceRange.Item2;

            return _storage.UpdateUserPreference(preference);
        }

        /***
         * Removes a price range from a category. 
         * Returns true if successful and false otherwise.
         * 
         * Can throw UserPreferenceNotFoundException if the category is not found.
         */
        public bool RemovePriceRange(string category)
        {
            UserPreference preference = FindUserPreferenceCache(category);
            preference._minPrice = 0.0;
            preference._maxPrice = 0.0;

            return _storage.UpdateUserPreference(preference);
        }

        /***
         * Retrieves the price range for a specific category. 
         * If there is not price range associated with a category then a tuple of (0.0, 0.0) is returned.
         * 
         * Can throw UserPreferenceNotFoundException is the category is not found.
         */
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
         * This exception is raised when the user preference cannot be found in the cache.
         */
        public class UserPreferenceNotFoundException : Exception
        {
            public UserPreferenceNotFoundException()
            {
            }

            public UserPreferenceNotFoundException(string message)
                : base(message)
            {
            }

            public UserPreferenceNotFoundException(string message, Exception exception)
                : base(message, exception)
            {
            }
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

            if (preference == null)
            {
                throw new UserPreferenceNotFoundException();
            }

            return preference;
        }
    }
}
