using System;
using Xunit;
using DiscordScraperBot;
using System.Collections.Generic;

namespace DiscordScraperBot.UnitTests
{
    public class PreferencesTests
    {
        [Fact]
        public void TestNullStorage()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                Preferences pref = new Preferences(null);
            });
        }

        [Fact]
        public void TestAddingNullCategory()
        {
            IStorage storage = new MockStorage();
            Preferences preferences = new Preferences(storage);

            Assert.Throws<ArgumentNullException>(() =>
            {
                preferences.AddCategory(null);
            });
        }

        [Fact] 
        public void TestAddingCategories()
        {
            IStorage storage = new MockStorage();
            Preferences preferences = new Preferences(storage);

            List<string> categories = new List<string>();

            categories.Add("gardening");
            categories.Add("electronics");
            categories.Add("test category");

            foreach (string category in categories)
            {
                preferences.AddCategory(category);
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
        public void TestRemovingNullCategory()
        {
            IStorage storage = new MockStorage();
            Preferences preferences = new Preferences(storage);

            Assert.Throws<ArgumentNullException>(() =>
            {
                preferences.RemoveCategory(null);
            });
        }

        [Fact]
        public void TestRemovingCategories()
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
            preferences.RemoveCategory("policeman");
            preferences.RemoveCategory("policewoman");

            List<UserPreference> mockPreferences = storage.GetUserPreferences();
            foreach (UserPreference pref in mockPreferences)
            {
                Assert.False(pref._category == "policeman" && pref._category == "policewoman");
            }
        }

        [Fact]
        public void TestAddingNullPriceRange()
        {
            IStorage storage = new MockStorage();
            Preferences preferences = new Preferences(storage);

            Assert.Throws<ArgumentNullException>(() =>
            {
                preferences.AddPriceRange(null, null);
            });
        }

        [Fact] 
        public void TestAddingPriceRange()
        {
            IStorage storage = new MockStorage();
            Preferences preferences = new Preferences(storage);

            List<string> categories = new List<string>();


        }

        /***
         * Ensures that the user is not able to add a price range for a NULL category.
         */
        [Fact]
        public void TestAddPriceWithNoCategory()
        {

        }

        /***
         * Ensures that the user is unable to add a price range for a catefory that does NOT exist.
         */
        [Fact]
        public void TestAddPriceWithFakeCategory()
        {

        }

        [Fact] 
        public void TestRemoveNullPriceRange()
        {
            IStorage storage = new MockStorage();
            Preferences preferences = new Preferences(storage);

            Assert.Throws<ArgumentNullException>(() =>
            {
                preferences.RemovePriceRange(null, null);
            });
        }

        [Fact]
        public void TestRemovePriceRange()
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

            public List<UserPreference> GetUserPreferences()
            {
                return _pref;
            }

            public bool InsertUserPreferences(List<UserPreference> preferences)
            {
                _pref.AddRange(preferences);
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
