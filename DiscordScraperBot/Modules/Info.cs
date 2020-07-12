using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace DiscordScraperBot.Modules
{
    public class Info : ModuleBase<SocketCommandContext>
    {
        /***
         * Displays the status of the bot. 
         * The information displayed includes:
         * 1. Number of items held by the bot.
         * 2. The date and time that each scraped last performed scraping.
         */
        [Command("info")]
        public async Task GetInfo()
        {
            var embed = new EmbedBuilder();

            embed.WithTitle("Current scrape rate: ");
            embed.WithDescription("info");
            embed.WithColor(new Color(10, 98, 234));

            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }

        /***
         * This method displays all the commands available for the user to use.
         */
        [Command("help")]
        public async Task ShowHelp()
        {
            
        }
    }
}
