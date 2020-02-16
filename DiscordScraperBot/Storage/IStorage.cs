using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordScraperBot
{
    public interface IStorage
    {
        public bool CreateTables();

        public bool CreatePreferenceTable();

        public List<UserPreference> GetUserPreferences();

        public UserPreference GetUserPreference(string category);

        public bool InsertUserPreferences(List<UserPreference> preferences);
        public bool InsertUserPreference(UserPreference preference);

        public bool DeleteUserPreferences(List<UserPreference> preferences);
        public bool DeleteUserPreference(UserPreference preference);

        public int GetNumberOfRows();

        public void CloseStorage();
    }
}
