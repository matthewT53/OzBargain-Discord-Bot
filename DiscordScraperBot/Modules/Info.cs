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
            var embed = new EmbedBuilder();

            embed.WithTitle("[+] Help:");
            embed.WithDescription("All the commands and their arguments are shown below: ");

            embed.AddField("Command Prefix", "\t\tThe command prefix is $", true);
            embed.AddField("help", "\t\tDisplays the available commands.", true);
            embed.AddField("info", "\t\tShows the info about this discord bot.", true);
            embed.AddField("show_scrape_rate", "\t\tDisplays how often web scraping will occur (milliseconds).", true);
            embed.AddField("set_scrape_rate <scrapte_rate>", "\t\tSets how often the scrapers will run (milliseconds).", true);
            embed.AddField("show_depth", "\t\tDisplays how many links each bot will follow to scrape.", true);
            embed.AddField("set_depth <bot index> <depth>", "\t\tChanges how many links the scraper will follow.", true);
            embed.AddField("show_post_delay", "\t\tDisplays how often bargains will be posted to the discord channel (milliseconds).", true);
            embed.AddField("set_post_delay <post delay>", "\t\tSets how often bargains will be posted to the discord channel (milliseconds).", true);

            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }
    }
}
