﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace DiscordScraperBot.UnitTests
{
    public class StorageTests : IDisposable
    {
        /***
         * The reason we don't use a fixture is that we want don't want the  
         * database to be shared across all tests otherwise dependecies may be 
         * created between test cases and they will not be able to run in isolation.
         */
        SqliteStorage _storage;

        const string TEST_DB_FOLDER = "Storage";
        const string TEST_DB_FILE = "test.sqlite";

        public StorageTests()
        { 
            _storage = new SqliteStorage(TEST_DB_FOLDER, TEST_DB_FILE);
        }

        public void Dispose()
        {
            _storage.CloseStorage();
            File.Delete(TEST_DB_FOLDER + "/" + TEST_DB_FILE);
        }

        /*
         * Ensure that we are able to create the preferences table
         */
        [Fact]
        public void CreateTableTest()
        {
            Assert.True(_storage.CreateTables());
        }

        /*
         * Ensure that creating a table twice is not possible.
         */
        [Fact]
        public void CreateTableTwiceTest()
        {
            Assert.True(_storage.CreateTables());
            Assert.False(_storage.CreateTables());
        }

        /*
        * Ensure that we can delete a created table successfully.
        */
        [Fact]
        public void DeleteTableTest()
        {
            Assert.True(_storage.CreateTables());
            Assert.True(_storage.DeletePreferenceTable());
        }

        /*
         * Ensure we cannot delete a table more than once.
         */
        [Fact]
        public void DeleteTableTwiceTest()
        {
            Assert.True(_storage.CreateTables());
            Assert.True(_storage.DeletePreferenceTable());
            Assert.True(_storage.DeletePreferenceTable());
        }

        /*
         * Ensure trying to insert null into the database.
         */
        [Fact]
        public void AddingNullPreferenceTest()
        {
            SqliteStorage storage = _storage;
            Assert.True(_storage.CreateTables());

            Assert.Throws<ArgumentNullException>(() =>
            {
                storage.InsertUserPreference(null);
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                storage.InsertUserPreferences(null);
            });
        }

        /*
         * Check that adding preferences works.
         */
        [Fact]
        public void AddingPreferencesTest()
        {
            SqliteStorage storage = _storage;
            Assert.True(_storage.CreateTables());

            List<UserPreference> preferences = new List<UserPreference>();

            preferences.Add( new UserPreference("electronics") );
            preferences.Add( new UserPreference("books", 10.0, 1900.0) );
            preferences.Add( new UserPreference("gardening", 50.0, 100.0) );

            bool result = storage.InsertUserPreferences(preferences);
            Assert.True(result);
        }

        /*
         * Ensures that we are able to add one preference to the database.
         */
        [Fact]
        public void AddingOnePreferenceTest()
        {
            SqliteStorage storage = _storage;
            Assert.True(_storage.CreatePreferenceTable());

            bool result = storage.InsertUserPreference(new UserPreference("headphones"));
            Assert.True(result);
        }

        /*
         * Ensure that deleting a null user preference is not allowed.
         */
        [Fact]
        public void RemovingNullPreferenceTest()
        {
            SqliteStorage storage = _storage;
            Assert.True(_storage.CreatePreferenceTable());

            bool result = storage.InsertUserPreference(new UserPreference("headphones"));
            Assert.True(result);

            Assert.Throws<ArgumentNullException>(() =>
            {
                storage.DeleteUserPreference(null);
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                storage.DeleteUserPreferences(null);
            });
        }

        /*
         * Check that removing preferences works:
         */
        [Fact]
        public void RemovingPreferencesTest()
        {
            SqliteStorage storage = _storage;
            Assert.True(_storage.CreateTables());

            List<UserPreference> preferences = new List<UserPreference>();

            preferences.Add(new UserPreference("electronics"));
            preferences.Add(new UserPreference("books", 10.0, 1900.0));
            preferences.Add( new UserPreference("gardening", 50.0, 100.0) );

            bool result = storage.InsertUserPreferences(preferences);
            Assert.True(result);

            result = storage.DeleteUserPreferences(preferences);
            Assert.True(result);
        }

        /*
         * Ensures that deleting one UserPreference works. 
         */
        [Fact]
        public void RemovingOnePreferenceTest()
        {
            SqliteStorage storage = _storage;
            Assert.True(_storage.CreatePreferenceTable());

            UserPreference preference = new UserPreference("headphones");
            bool result = storage.InsertUserPreference(preference);
            Assert.True(result);

            result = storage.DeleteUserPreference(preference);
            Assert.True(result);
        }

        /*
         * Check that are able to retrieve user preferences from the database.
         */
        [Fact]
        public void RetievePreferencesTest()
        {
            SqliteStorage storage = _storage;
            Assert.True(_storage.CreateTables());

            List<UserPreference> preferences = new List<UserPreference>();

            preferences.Add(new UserPreference("movies", 0.0, 0.0));
            preferences.Add(new UserPreference("games", 0.0, 100.0));

            bool result = storage.InsertUserPreferences(preferences);
            Assert.True(result);

            List<UserPreference> myPreferences = storage.GetUserPreferences();
            Assert.True(myPreferences.Count == preferences.Count);

            for (int i = 0; i < myPreferences.Count; i++)
            {
                Assert.Equal(preferences[i], myPreferences[i]);
            }
        }

        /*
         * Ensure that we handle updating null references properly.
         */
        [Fact]
        public void UpdateNullPreferencesTest()
        {
            SqliteStorage storage = _storage;
            Assert.True(_storage.CreatePreferenceTable());

            Assert.Throws<ArgumentNullException>(() =>
            {
                _storage.UpdateUserPreference(null);
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                _storage.UpdateUserPreferences(null);
            });
        }

        /*
         * Check to see if we can update a UserPreference
         */
        [Fact]
        public void UpdateOnePreferenceTest() 
        {
            SqliteStorage storage = _storage;
            Assert.True(_storage.CreatePreferenceTable());

            List<UserPreference> preferences = new List<UserPreference>();

            // When is the primary key set, at inserion or object creation?
            UserPreference u1 = new UserPreference("movies", 0.0, 0.0);
            UserPreference u2 = new UserPreference("games", 0.0, 100.0);
            preferences.Add(u1);
            preferences.Add(u2);

            bool result = storage.InsertUserPreferences(preferences);
            Assert.True(result);

            u1._category = "gardening";
            u1._minPrice = 95.0;
            u1._maxPrice = 165.75;
            result = storage.UpdateUserPreference(u1);
            Assert.True(result);

            UserPreference updatedPref = storage.GetUserPreference("gardening");
            Assert.NotNull(updatedPref);

            Assert.Equal(95.0, updatedPref._minPrice);
            Assert.Equal(165.75, updatedPref._maxPrice);
        }

        /*
         * Ensure we can update many UserPreference records.
         */
        [Fact]
        public void UpdatePreferencesTest() 
        {
            SqliteStorage storage = _storage;
            Assert.True(_storage.CreatePreferenceTable());

            List<UserPreference> preferences = new List<UserPreference>();

            // When is the primary key set, at inserion or object creation?
            UserPreference u1 = new UserPreference("movies", 0.0, 0.0);
            UserPreference u2 = new UserPreference("games", 0.0, 100.0);
            preferences.Add(u1);
            preferences.Add(u2);

            bool result = storage.InsertUserPreferences(preferences);
            Assert.True(result);

            preferences[0]._category = "gardening";
            preferences[0]._minPrice = 95.0;
            preferences[0]._maxPrice = 165.75;

            preferences[1]._category = "electrical";
            preferences[1]._minPrice = 190.50;
            preferences[1]._maxPrice = 567.90;

            result = storage.UpdateUserPreferences(preferences);
            Assert.True(result);

            UserPreference updatedPref = storage.GetUserPreference("gardening");

            Assert.NotNull(updatedPref);
            Assert.Equal(95.0, updatedPref._minPrice);
            Assert.Equal(165.75, updatedPref._maxPrice);

            updatedPref = storage.GetUserPreference("electrical");
            Assert.Equal(190.50, updatedPref._minPrice);
            Assert.Equal(567.90, updatedPref._maxPrice);
        }

        /*
         * Ensure that we can correct determine the number of rows in a database.
         */
        [Fact]
        public void GetNumberOfRowsTest()
        {
            SqliteStorage storage = _storage;
            Assert.True(_storage.CreateTables());

            List<UserPreference> preferences = new List<UserPreference>();

            preferences.Add(new UserPreference("movies", 0.0, 0.0));
            preferences.Add(new UserPreference("games", 0.0, 100.0));
            preferences.Add(new UserPreference("tools", 0.0, 100.0));

            bool result = storage.InsertUserPreferences(preferences);
            Assert.True(result);
            Assert.True(storage.GetNumberOfRows() == preferences.Count);

            int numItems = preferences.Count;
            preferences.RemoveAt(2);
            result = storage.DeleteUserPreferences(preferences);
            Assert.True(result);
            Assert.True(storage.GetNumberOfRows() == (numItems - preferences.Count));
        }

        /*
         * Check that we can correctly retrieve a desired UserPreference.
         */
        [Fact]
        public void GetUserPreferenceTest()
        {
            SqliteStorage storage = _storage;
            Assert.True(_storage.CreateTables());

            List<UserPreference> preferences = new List<UserPreference>();

            preferences.Add(new UserPreference("movies", 0.0, 0.0));
            preferences.Add(new UserPreference("games", 0.0, 100.0));
            preferences.Add(new UserPreference("tools", 0.0, 100.0));

            bool result = storage.InsertUserPreferences(preferences);
            Assert.True(result);
            Assert.True(storage.GetNumberOfRows() == preferences.Count);

            UserPreference userPreference = storage.GetUserPreference("games");
            Assert.NotNull(userPreference);
            Assert.Equal("games", userPreference._category);
            Assert.True(userPreference._minPrice == 0.0);
            Assert.True(userPreference._maxPrice == 100.0);
        }

        /*
         * Check that grabbing a user preference that doesn't exist returns null.
         */
        [Fact]
        public void GetUserPreferenceNullTest()
        {
            SqliteStorage storage = _storage;
            Assert.True(_storage.CreateTables());

            List<UserPreference> preferences = new List<UserPreference>();

            preferences.Add(new UserPreference("movies", 0.0, 0.0));
            preferences.Add(new UserPreference("games", 0.0, 100.0));
            preferences.Add(new UserPreference("tools", 0.0, 100.0));

            bool result = storage.InsertUserPreferences(preferences);
            Assert.True(result);
            Assert.True(storage.GetNumberOfRows() == preferences.Count);

            UserPreference userPreference = storage.GetUserPreference("garden");
            Assert.Null(userPreference);
        }
    
    }
}
