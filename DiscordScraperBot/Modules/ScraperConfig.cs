using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using DiscordScraperBot.Discord;

namespace DiscordScraperBot.Modules
{
    public class ScraperConfig : ModuleBase<SocketCommandContext>
    {
        private ScraperManager Manager;

        /*
         * The scraper manager is required to acquire items that were scraped.
         */
        public ScraperConfig(ScraperManager scraperManager)
        {
            Console.Out.WriteLine("[+] Inside scraper module constructor");
            Manager = scraperManager;

            Console.Out.WriteLine("[+] Manager hashcode: " + Manager.GetHashCode());
        }

        /*
         * The user can issue this command to show the delay in milliseconds 
         * between scraping the Ozbargain website.
         */
        [Command("show_scrape_rate")]
        public async Task ShowRate()
        {
            int rate = Manager.GetDelay();

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
        [Command("set_scrape_rate")]
        public async Task ChangeRate([Remainder] int new_rate)
        {
            Manager.SetDelay(new_rate);

            var embed = new EmbedBuilder();
            embed.WithTitle("Changed scrape rate to: ");
            embed.WithDescription(new_rate.ToString());
            embed.WithColor(new Color(10, 98, 234));

            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }

        [Command("show_depth")]
        public async Task ShowDepth()
        {
            var embed = new EmbedBuilder();
            embed.WithTitle("Depths:");
            embed.WithColor(new Color(10, 98, 234));

            var scrapers = Manager.GetScrapers();
            foreach (var scraper in scrapers)
            {
                int depth = scraper.GetDepth();
                embed.AddField(scraper.GetName(), depth == Int32.MaxValue ? "NO limit" : depth.ToString(), true);
            }

            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }

        [Command("set_depth")]
        public async Task ChangeDepth([Remainder] int botIndex, [Remainder] int depth)
        {
            var scrapers = Manager.GetScrapers();
            scrapers[botIndex].SetDepth(depth);

            var embed = new EmbedBuilder();
            embed.WithTitle("Changed depth of: " + scrapers[botIndex].GetName() + " to:");
            embed.WithDescription(depth.ToString());
            embed.WithColor(new Color(10, 98, 234));

            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }
    }
}
