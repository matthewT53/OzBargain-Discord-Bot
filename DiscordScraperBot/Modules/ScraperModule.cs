using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace DiscordScraperBot.Modules
{
    public class ScraperModule : ModuleBase<SocketCommandContext>
    {
        private readonly ScraperManager _manager;
        public ScraperModule(ScraperManager scraper_manager)
        {
            Console.Out.WriteLine("[+] Inside scraper module constructor");
            _manager = scraper_manager;
        }

        /*
         * The user can issue this command to the delay between scraping 
         * the Ozbargain website.
         */
        [Command("scraperate")]
        public async Task ChangeRate()
        {
            var embed = new EmbedBuilder();

            embed.WithTitle("Current scrape rate: ");
            embed.WithColor(new Color(10, 98, 234));

            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }
    }
}
