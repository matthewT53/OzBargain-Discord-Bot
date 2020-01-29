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
    }

    public class Storage
    {
        SQLiteConnection _db;

        const string DbFolder = "Storage";
        const string DbFilename = "pref.sqlite";
        const string DbPath = DbFolder + DbFilename;

        public Storage()
        {
            _db = new SQLiteConnection(DbPath);
        }

        public bool CreatePreferenceTable()
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
