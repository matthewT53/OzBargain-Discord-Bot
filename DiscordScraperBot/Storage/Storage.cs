using SQLite;
using System.Collections.Generic;

namespace DiscordScraperBot
{
    public class UserPreference
    {
        [PrimaryKey, AutoIncrement]
        public int _id { get; set; }
        public string _category { get; set; }

        public double _minPrice { get; set; }
        public double _maxPrice { get; set; }

        public UserPreference(string category)
        {
            _category = category;
            _minPrice = 0.0;
            _maxPrice = 0.0;
        }

        public UserPreference(string category, double minPrice, double maxPrice)
        {
            _category = category;
            _minPrice = minPrice;
            _maxPrice = maxPrice;
        }

    }

    public class Storage
    {
        SQLiteConnection _db;

        const string DefaultDbFolder = "Storage";
        const string DefaultDbFilename = "pref.sqlite";
        const string DefaultDbPath = DefaultDbFolder + DefaultDbFilename;

        public Storage()
        {
            _db = new SQLiteConnection(DefaultDbPath);
        }

        public Storage(string dbPath)
        {
            _db = new SQLiteConnection(dbPath);
        }

        public bool CreatePreferenceTable()
        {
            return false;
        }

        public bool DeletePreferenceTable()
        {
            return false;
        }

        public List<UserPreference> GetUserPreferences()
        {
            return null;
        }

        public bool InsertUserPreferences(List<UserPreference> preferences)
        {
            return false;
        }

        public bool DeleteUserPreferences(List<UserPreference> preferences)
        {
            return false;
        }

        public bool DestroyStorage()
        {
            return false;
        }
    }
}
