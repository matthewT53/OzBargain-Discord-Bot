using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordScraperBot
{
    /*
     * The purpose of this class is to store the user's shopping preferences 
     * which includes the categories they are after (e.g electronics , clothing, etc)
     * as well as price range.
     * 
     * These preferences are stored in a file for now however an sqlite database may be 
     * utilized later if needed.
     */
    public class Preferences
    {
        List<string> categories_;
        public Preferences()
        {

        }


    }
}
