using System;

namespace DiscordScraperBot
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Config.bot.token);
            Console.WriteLine(Utilities.GetAlert("Test"));
        }
    }
}
