using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordScraperBot
{
    public interface IStorage
    {
        public bool CreateTables();

        public List<UserPreference> GetUserPreferences();

        public UserPreference GetUserPreference(string category);

        public bool InsertUserPreferences(List<UserPreference> preferences);

        public bool DeleteUserPreferences(List<UserPreference> preferences);

        public int GetNumberOfRows();

        public void CloseStorage();
    }
}
