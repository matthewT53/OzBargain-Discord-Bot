using SQLite;
using System;
using System.Collections.Generic;
using System.IO;

namespace DiscordScraperBot
{
    public class UserPreference
    {
        [PrimaryKey, AutoIncrement]
        public int _id { get; set; }
        public string _category { get; set; }
        public double _minPrice { get; set; }
        public double _maxPrice { get; set; }

        public UserPreference()
        {
            _id = 0;
            _category = "(null)";
            _minPrice = 0.0;
            _maxPrice = 0.0;
        }

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

        public override bool Equals(object obj)
        {
            UserPreference pref = obj as UserPreference;

            if (pref == null)
            {
                return false;
            }

            return  (_category == pref._category) &&
                    (_maxPrice == pref._maxPrice) &&
                    (_minPrice == pref._minPrice);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
 
    public class SqliteStorage : IStorage
    {
        /***
        * The following links may help with using the Sqlite-net library:
        * https://github.com/praeclarum/sqlite-net/blob/master/src/SQLite.cs
        */
        SQLiteConnection _db;

        const string DefaultDbFolder = "Storage";
        const string DefaultDbFilename = "pref.sqlite";
        const string DefaultDbPath = DefaultDbFolder + "/" + DefaultDbFilename;

        public SqliteStorage()
        {
            CreateSqliteFile(DefaultDbFolder, DefaultDbFilename);
            _db = new SQLiteConnection(DefaultDbPath, SQLiteOpenFlags.Create | SQLiteOpenFlags.ReadWrite);
        }

        public SqliteStorage(string dbPath, string dbFilename)
        {
            CreateSqliteFile(dbPath, dbFilename);

            string dbFullPath = dbPath + "/" + dbFilename;
            _db = new SQLiteConnection(dbFullPath, SQLiteOpenFlags.Create | SQLiteOpenFlags.ReadWrite);
        }

        public bool CreateTables()
        {
            return CreatePreferenceTable();
        }

        public bool CreatePreferenceTable()
        {
            CreateTableResult result = _db.CreateTable<UserPreference>();
            return result == CreateTableResult.Created;
        }

        public bool DeletePreferenceTable()
        {
            /*
             * Drop table returns the number of rows deleted from the table.
             */
            return _db.DropTable<UserPreference>() >= 0;
        }

        public List<UserPreference> GetUserPreferences()
        {
            List<UserPreference> result = _db.Table<UserPreference>().ToList();
            return result;
        }

        public UserPreference GetUserPreference(string category) 
        {
            List<UserPreference> preferences = _db.Query<UserPreference>("select * from UserPreference where _category = ?", category);
            return (preferences.Count == 1) ? preferences[0] : null;
        }

        public bool InsertUserPreferences(List<UserPreference> preferences)
        {
            if (preferences == null)
            {
                throw new ArgumentNullException("preferences list cannot be null.");
            }

            int nRows = _db.InsertAll(preferences);
            return nRows == preferences.Count;
        }

        public bool InsertUserPreference(UserPreference preference)
        {
            if (preference == null)
            {
                throw new ArgumentNullException("preference cannot be null.");
            }

            int nRows = _db.Insert(preference);
            return nRows == 1;
        }

        public bool DeleteUserPreferences(List<UserPreference> preferences)
        {
            if (preferences == null)
            {
                throw new ArgumentNullException("preferences cannot be null.");
            }

            int nRows = 0;
            foreach (UserPreference userPreference in preferences)
            {
                nRows += _db.Delete<UserPreference>(userPreference._id);
            }

            return nRows == preferences.Count;
        }

        public bool DeleteUserPreference(UserPreference preference)
        {
            if (preference == null)
            {
                throw new ArgumentNullException("preference cannot be null.");
            }

            int nRows = _db.Delete<UserPreference>(preference._id);
            return nRows == 1;
        }

        public bool UpdateUserPreference(UserPreference preference)
        {
            if (preference == null)
            {
                throw new ArgumentNullException("preference cannot be null.");
            }

            Console.WriteLine("[UpdateUserPreference] preference.id = " + preference._id);

            int nRows = _db.Update(preference);
            return nRows == 1;
        }

        public bool UpdateUserPreferences(List<UserPreference> preferences)
        {
            if (preferences == null)
            {
                throw new ArgumentNullException("preferences cannot be null.");
            }

            int nRows = _db.UpdateAll(preferences);
            return nRows == preferences.Count;
        }

        public int GetNumberOfRows()
        {
            return _db.Table<UserPreference>().Count();
        }

        public void CloseStorage()
        {
            _db.Close();
        }

        private void CreateSqliteFile(string path, string filename)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string fullPath = path + "/" + filename;
            if (!File.Exists(fullPath))
            {
                FileStream fileStream = File.Create(fullPath);
                fileStream.Close();
            }
        }
    }
}
