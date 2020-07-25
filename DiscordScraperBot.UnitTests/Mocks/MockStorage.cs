using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordScraperBot.UnitTests
{
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
            foreach (UserPreference preference in preferencesToUpdate)
            {
                if (!UpdateUserPreference(preference))
                {
                    return false;
                }
            }

            return true;
        }

        public bool UpdateUserPreference(UserPreference preference)
        {
            int index = _pref.FindIndex((u) =>
            {
                return u._id == preference._id && u._category == preference._category;
            });

            if (index < 0)
            {
                return false;
            }

            _pref[index]._minPrice = preference._minPrice;
            _pref[index]._maxPrice = preference._maxPrice;
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
