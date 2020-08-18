using System;
using System.Collections.Generic;
using System.Linq;
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
        IStorage Storage;

        // Cache layer for user preferences
        Dictionary<string, UserPreference> CachedPreferences { get; set; }

        public Preferences(IStorage storage)
        {
            if (storage == null)
            {
                throw new ArgumentNullException("storage cannot be null.");
            }
            Storage = storage;
            Storage.CreatePreferenceTable();

            CachedPreferences = new Dictionary<string, UserPreference>();

            // Load the existing preferences from the database.
            List<UserPreference> userPreferences = Storage.GetUserPreferences();
            foreach (UserPreference preference in userPreferences)
            {
                CachedPreferences.Add(preference._category, preference);
            }
        }

        /***
         * Retrieves a UserPreference object from the cache that corresponds to the filter.
         * i.e filter == preference._category
         * 
         * Parameters:
         * [out] preference : UserPreference
         * Returns true if a user preference object was found and false otherwise.
         */
        public bool FindUserPreferenceFromCache(string filter, out UserPreference preference)
        {
            //return CachePreferences.TryGetValue(filter, out preference);
            preference = null;
            return false;
        }

        /****
         * Returns a list of UserPreferences objects stored in the cache.
         * 
         * Returns:
         * @List<UserPreference>
         */
        public List<UserPreference> GetUserPreferences()
        {
            return CachedPreferences.Values.ToList();
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
            CachedPreferences.Add(category, preference);
            return Storage.InsertUserPreference(preference);
        }

        /***
         * Creates a UserPreference object with a price range for a category and then inserts into the database.
         * Returns true if the UserPreference was successfully added into the storage and false otherwise.
         */
        public bool AddCategory(string category, Tuple<double, double> priceRange)
        {
            UserPreference preference = new UserPreference(category, priceRange.Item1, priceRange.Item2);
            CachedPreferences.Add(category, preference);
            return Storage.InsertUserPreference(preference);
        }

        /***
         * Removes a UserPreference from the storage corresponding to the given category.
         * Returns true if the removal was successful from the underlying storage and false otherwise.
         */
        public bool RemoveCategory(string category)
        {
            UserPreference preference;
            bool result = FindUserPreferenceFromCache(category, out preference);
            if (result)
            {
                CachedPreferences.Remove(category);
                result = Storage.DeleteUserPreference(preference);
            }
            return result;
        }

        /***
         * Associates a price range for an existing category.
         * Returns true if successful and false otherwise.
         * 
         * Throws:
         * 1. ArgumentNullException - If priceRange is null.
         * 2. UserPreferenceNotFoundException - If category is not found.
         */
        public bool AddPriceRange(string category, Tuple<double, double> priceRange)
        {
            UserPreference preference;
            bool result = FindUserPreferenceFromCache(category, out preference);

            if (priceRange == null)
            {
                throw new ArgumentNullException("priceRange cannot be null.");
            }

            if (result)
            {
                preference._minPrice = priceRange.Item1;
                preference._maxPrice = priceRange.Item2;
            }

            return Storage.UpdateUserPreference(preference);
        }

        /***
         * Removes a price range from an existing category. 
         * Returns true if successful and false otherwise.
         * 
         * Can throw UserPreferenceNotFoundException if the category is not found.
         */
        public bool RemovePriceRange(string category)
        {
            UserPreference preference;
            bool result = FindUserPreferenceFromCache(category, out preference);

            if (result)
            {
                preference._minPrice = 0.0;
                preference._maxPrice = 0.0;
            }

            return Storage.UpdateUserPreference(preference);
        }

        /***
         * Retrieves the price range for an existing category. 
         * If there is not price range associated with a category then a tuple of (0.0, 0.0) is returned.
         * 
         * Can throw UserPreferenceNotFoundException is the category is not found.
         */
        public Tuple<double, double> GetPriceRange(string category)
        {
            UserPreference preference;
            bool result = FindUserPreferenceFromCache(category, out preference);
            if (!result)
            {
                throw new UserPreferenceNotFoundException();
            }

            Tuple<double, double> priceRange = new Tuple<double, double>(preference._minPrice, preference._maxPrice);
            return priceRange;
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
    }
}
