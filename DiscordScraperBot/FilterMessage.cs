using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DiscordScraperBot.BotMessages;

namespace DiscordScraperBot
{
    public class FilterMessage
    {
        Preferences UserPreferences { get; set; }

        public FilterMessage(Preferences preferences)
        {
            UserPreferences = preferences;
        }

        /***
        * Determines if a message is desirable to the user. 
        * @Returns: 
        *  true if the message should be posted because it matches a filter applied by the user.
        *  false otherwise.
        */
        public bool IsDesirable(IBotMessage message)
        {
            bool desirable = false;
            // No filters have been created by the user so we accept the message.
            if (UserPreferences.Count() == 0)
            {
                Console.WriteLine("[+] No filters applied so we take all messages!");
                return true;
            }

            else
            {
                string[] titleHints = message.Name.Split(' ');
                List<string> filterItems = titleHints.ToList<string>();
                filterItems.AddRange(message.Categories);

                foreach (string filterItem in filterItems)
                {
                    Console.WriteLine("[+] Considering category: " + filterItem);
                    UserPreference preference;

                    if (UserPreferences.FindUserPreferenceFromCache(filterItem.ToLower(), out preference))
                    {
                        Console.WriteLine("[+] Category matches, considering prices!");
                        if (preference._minPrice != 0.0 && preference._maxPrice != 0.0)
                        {
                            double price = Convert.ToDouble(message.Price);
                            desirable = (price >= preference._minPrice && price <= preference._maxPrice);
                        }

                        else
                        {
                            desirable = true;
                        }

                        break;
                    }
                }
            }

            return desirable;
        }
    }
}
