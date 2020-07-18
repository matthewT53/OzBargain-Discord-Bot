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
        [Command("show_scrape_delay")]
        public async Task ShowRate()
        {
            int rate = Manager.Delay / 1000;

            var embed = new EmbedBuilder();
            embed.WithTitle("This bot scrapes every:  ");
            embed.WithDescription(rate.ToString() + " seconds.");
            embed.WithColor(Color.Orange);

            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }

        /*
         * The user can issue this command to modify the delay between scraping
         * the ozbargain website.
         */
        [Command("set_scrape_delay")]
        public async Task ChangeRate([Remainder] int newRate)
        {
            int rate = newRate * 1000;
            Manager.Delay = rate;

            var embed = new EmbedBuilder();
            embed.WithTitle("The scraper will run every: ");
            embed.WithDescription(newRate.ToString() + " seconds.");
            embed.WithColor(Color.Orange);

            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }

        [Command("show_depth")]
        public async Task ShowDepth()
        {
            var embed = new EmbedBuilder();
            embed.WithTitle("Depths:");
            embed.WithColor(Color.Orange);

            var scrapers = Manager.GetScrapers();
            foreach (var scraper in scrapers)
            {
                int depth = scraper.Depth;
                embed.AddField(scraper.Name, depth == Int32.MaxValue ? "No limit" : depth.ToString(), true);
            }

            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }

        [Command("set_depth")]
        public async Task ChangeDepth(int botIndex, [Remainder] int depth)
        {
            var scrapers = Manager.GetScrapers();
            if (botIndex >= 0 && botIndex < scrapers.Count && depth > 0)
            {
                scrapers[botIndex].Depth = depth;

                var embed = new EmbedBuilder();
                embed.WithTitle("Changed depth of " + scrapers[botIndex].Name + " to:");
                embed.WithDescription(depth.ToString() + " levels.");
                embed.WithColor(Color.Orange);

                await Context.Channel.SendMessageAsync("", false, embed.Build());
            }
        }
    }
}
