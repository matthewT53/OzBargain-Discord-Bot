using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace DiscordScraperBot.Modules
{
    public class Scrapers : ModuleBase<SocketCommandContext>
    {
        private ScraperManager _manager;

        /*
         * The scraper manager is required to acquire items that were scraped.
         */
        public Scrapers(ScraperManager scraper_manager)
        {
            Console.Out.WriteLine("[+] Inside scraper module constructor");
            _manager = scraper_manager;

            Console.Out.WriteLine("[+] _manager hashcode: " + _manager.GetHashCode());
        }

        /*
         * The user can issue this command to show the delay in milliseconds 
         * between scraping the Ozbargain website.
         */
        [Command("show_rate")]
        public async Task ShowRate()
        {
            Console.Out.WriteLine("[+] Inside ChangeRate(), _manager hashcode: " + _manager.GetHashCode());
            int rate = _manager.GetDelay();

            var embed = new EmbedBuilder();
            embed.WithTitle("Current scrape rate: ");
            embed.WithDescription(rate.ToString());
            embed.WithColor(new Color(10, 98, 234));

            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }

        /*
         * The user can issue this command to modify the delay between scraping
         * the ozbargain website.
         */
        [Command("change_rate")]
        public async Task ChangeRate([Remainder] int new_rate)
        {
            Console.WriteLine("[+] Changing scrapte rate to: " + new_rate);
            _manager.SetDelay(new_rate);

            var embed = new EmbedBuilder();
            embed.WithTitle("Changed scrape rate to: ");
            embed.WithDescription(new_rate.ToString());
            embed.WithColor(new Color(10, 98, 234));

            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }
    }
}
