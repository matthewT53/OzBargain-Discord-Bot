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
        [Command("info")]
        public async Task GetInfo()
        {
            var embed = new EmbedBuilder();

            embed.WithTitle("Current scrape rate: ");
            embed.WithDescription("info");
            embed.WithColor(new Color(10, 98, 234));

            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }
    }
}
