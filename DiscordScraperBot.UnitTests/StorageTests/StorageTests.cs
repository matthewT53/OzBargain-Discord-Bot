using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace DiscordScraperBot.UnitTests.StorageTests
{
    public class StorageFixture : IDisposable
    {
        public Storage _storage { get; private set; }

        const string TEST_DB_PATH = "Storage/test.sqlite";

        public StorageFixture()
        {
            _storage = new Storage(TEST_DB_PATH);
            _storage.CreatePreferenceTable();
        }

        public void Dispose()
        {
            File.Delete(TEST_DB_PATH);
        }
    }

    public class StorageTests : IClassFixture<StorageFixture>
    { 
        /*
         * The reason we create a seperate class to test the creation and deletion 
         * of tables is to increase the performance of our tests. 
         * 
         * For these test cases, a fixture is not appropriate because the tables 
         * in the database are constantly created and deleted between each test
         * which is extremely slow.
         */
        public class StorageTableCreation : IDisposable
        {
            Storage _storage;

            const string TEST_DB_PATH = "Storage/test.sqlite";

            public StorageTableCreation()
            {
                _storage = new Storage(TEST_DB_PATH);
            }

            public void Dispose()
            {
                File.Delete(TEST_DB_PATH);
            }

            /*
             * Ensure that we are able to create the preferences table
             */
            [Fact]
            public void CreateTableTest()
            {
                Assert.True(_storage.CreatePreferenceTable());
            }

            /*
             * Ensure that creating a table twice is not possible.
             */
            [Fact]
            public void CreateTableTwiceTest()
            {
                Assert.True(_storage.CreatePreferenceTable());
                Assert.False(_storage.CreatePreferenceTable());
            }

            /*
            * Ensure that we can delete a created table successfully.
            */
            [Fact]
            public void DeleteTableTest()
            {
                Assert.True(_storage.CreatePreferenceTable());
                Assert.True(_storage.DeletePreferenceTable());
            }

            /*
             * Ensure we cannot delete a table more than once.
             */
            [Fact]
            public void DeleteTableTwiceTest()
            {
                Assert.True(_storage.CreatePreferenceTable());
                Assert.True(_storage.DeletePreferenceTable());
                Assert.False(_storage.DeletePreferenceTable());
            }
        }

        StorageFixture _storageFixture;

        public StorageTests(StorageFixture fixture)
        {
            _storageFixture = fixture;
        }
        
        /*
         * Check that adding preferences works.
         */
        [Fact]
        public void AddingPreferencesTest()
        {
            Storage storage = _storageFixture._storage;

            List<UserPreference> preferences = new List<UserPreference>();

            preferences.Add(new UserPreference("electronics"));
            preferences.Add(new UserPreference("books", 10.0, 1900.0));
            preferences.Add(new UserPreference("gardening", 50.0, 100.0));

            bool result = storage.InsertUserPreferences(preferences);
            Assert.True(result);
        }

        /*
         * Check that removing preferences works:
         */
        public void RemovingPreferencesTest()
        {

        }



    }
}
