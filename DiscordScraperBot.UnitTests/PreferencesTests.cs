using System;
using Xunit;
using DiscordScraperBot;
using System.Collections.Generic;

namespace DiscordScraperBot.UnitTests
{
    public class PreferencesTests
    {
        [Fact]
        public void NullStorageTest()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                Preferences pref = new Preferences(null);
            });
        }

        [Fact]
        public void AddingNullCategoryTest()
        {
            IStorage storage = new MockStorage();
            Preferences preferences = new Preferences(storage);

            Assert.Throws<ArgumentNullException>(() =>
            {
                preferences.AddCategory(null);
            });
        }

        [Fact] 
        public void AddingCategoriesTest()
        {
            IStorage storage = new MockStorage();
            Preferences preferences = new Preferences(storage);

            List<string> categories = new List<string>();

            categories.Add("gardening");
            categories.Add("electronics");
            categories.Add("test category");

            foreach (string category in categories)
            {
                Assert.True( preferences.AddCategory(category) );
            }

            // Ensure these categories have been added to the mock 
            List<UserPreference> mockPreferences = storage.GetUserPreferences();

            foreach (string category in categories)
            {
                bool found = false;
                foreach (UserPreference pref in mockPreferences)
                {
                    if (pref._category == category)
                    {
                        found = true;
                    }
                }

                Assert.True(found);
            }
        }

        [Fact]
        public void RemovingNullCategoryTest()
        {
            IStorage storage = new MockStorage();
            Preferences preferences = new Preferences(storage);

            Assert.Throws<ArgumentNullException>(() =>
            {
                preferences.RemoveCategory(null);
            });
        }

        [Fact]
        public void RemovingCategoriesTest()
        {
            IStorage storage = new MockStorage();
            Preferences preferences = new Preferences(storage);

            // Add some categories to the mock storage
            List<string> categories = new List<string>();

            categories.Add("gardening");
            categories.Add("electronics");
            categories.Add("test category");
            categories.Add("policeman");
            categories.Add("policewoman");

            foreach (string category in categories)
            {
                preferences.AddCategory(category);
            }

            // Remove some categories 
            Assert.True( preferences.RemoveCategory("policeman") );
            Assert.True( preferences.RemoveCategory("policewoman") );

            List<UserPreference> mockPreferences = storage.GetUserPreferences();
            foreach (UserPreference pref in mockPreferences)
            {
                Assert.False(pref._category == "policeman" && pref._category == "policewoman");
            }
        }

        [Fact]
        public void AddingNullPriceRangeTest()
        {
            IStorage storage = new MockStorage();
            Preferences preferences = new Preferences(storage);

            Assert.Throws<ArgumentNullException>(() =>
            {
                preferences.AddPriceRange(null, null);
            });
        }

        /***
         * Ensures that a price range can be correctly applied to a category. 
         */
        [Fact] 
        public void AddingPriceRangeTest()
        {
            IStorage storage = new MockStorage();
            Preferences preferences = new Preferences(storage);

            List<string> categories = new List<string>();

            categories.Add("test_cat");
            categories.Add("test_cat2");
            categories.Add("test_cat3");

            Tuple<double, double> priceRange = new Tuple<double, double>(10.0, 100.0);

            foreach (string category in categories) 
            {
                preferences.AddCategory(category);
            }

            preferences.AddPriceRange("test_cat2", priceRange);

            UserPreference userPreference = storage.GetUserPreference("test_cat2");
            Assert.Equal("test_cat2", userPreference._category);
            Assert.True(userPreference._minPrice == 10.0);
            Assert.True(userPreference._maxPrice == 100.0);
        }

        /***
         * Ensures that the user is not able to add a price range for a NULL category.
         */
        [Fact]
        public void AddPriceWithNoCategoryTest()
        {
            IStorage storage = new MockStorage();
            Preferences preferences = new Preferences(storage);

            Tuple<double, double> priceRange = new Tuple<double, double>(10.0, 100.0);
            Assert.Throws<ArgumentNullException>(() =>
            {
                preferences.AddPriceRange(null, priceRange);
            });
        }

        /***
         * Ensures that the user is unable to add a price range for a catefory that does NOT exist.
         */
        [Fact]
        public void AddPriceWithFakeCategoryTest()
        {
            IStorage storage = new MockStorage();
            Preferences preferences = new Preferences(storage);

            Tuple<double, double> priceRange = new Tuple<double, double>(10.0, 100.0);

            Assert.False( preferences.AddPriceRange("fake_category", priceRange) );
        }

        [Fact] 
        public void RemoveNullPriceRangeTest()
        {
            IStorage storage = new MockStorage();
            Preferences preferences = new Preferences(storage);

            Assert.Throws<ArgumentNullException>(() =>
            {
                preferences.RemovePriceRange(null);
            });
        }

        [Fact]
        public void RemovePriceRangeTest()
        {
            IStorage storage = new MockStorage();
            Preferences preferences = new Preferences(storage);

            List<string> categories = new List<string>();

            categories.Add("test_cat");
            categories.Add("test_cat2");
            categories.Add("test_cat3");

            Tuple<double, double> priceRange = new Tuple<double, double>(10.0, 100.0);

            foreach (string category in categories) 
            {
                preferences.AddCategory(category);
                preferences.AddPriceRange(category, priceRange);
            }

            Assert.True( preferences.RemovePriceRange("test_cat2") );

            UserPreference userPreference = storage.GetUserPreference("test_cat2");
            Assert.Null( userPreference );
        }

        /*
         * Ensure that we can retrieve price ranges for categories.
         */
        [Fact]
        public void RetrievePriceRangeTest()
        {

        }

        public class MockStorage : IStorage
        {
            List<UserPreference> _pref;

            public MockStorage()
            {
                _pref = new List<UserPreference>();
            }

            public bool CreateTables()
            {
                return true;
            }

            public bool CreatePreferenceTable()
            {
                return true;
            }

            public List<UserPreference> GetUserPreferences()
            {
                return _pref;
            }

            public UserPreference GetUserPreference(string category)
            {
                return _pref.Find((userPref) =>
                {
                    return userPref._category == category;
                });
            }

            public bool InsertUserPreferences(List<UserPreference> preferences)
            {
                _pref.AddRange(preferences);
                return true;
            }

            public bool InsertUserPreference(UserPreference preference)
            {
                _pref.Add(preference);
                return true;
            }

            public bool DeleteUserPreferences(List<UserPreference> preferencesToDelete)
            {
                foreach (UserPreference pref in preferencesToDelete)
                {
                    _pref.Remove(pref);
                }

                return true;
            }

            public bool DeleteUserPreference(UserPreference preference)
            {
                _pref.Remove(preference);
                return true;
            }

            public bool UpdateUserPreferences(List<UserPreference> preferencesToUpdate)
            {
                return false;
            }

            public bool UpdateUserPreference(UserPreference preference)
            {
                return false;
            }

            public int GetNumberOfRows()
            {
                return _pref.Count;
            }

            public void CloseStorage()
            {
            }
        }
    }
}
