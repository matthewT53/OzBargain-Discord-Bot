using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace DiscordScraperBot.Modules
{
    public class Misc : ModuleBase<SocketCommandContext>
    {
        [Command("echo")]
        public async Task echo([Remainder]string message)
        {
            var embed = new EmbedBuilder();

            embed.WithTitle("Your message:");
            embed.WithDescription(message);
            embed.WithColor(new Color(10, 98, 234));

            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }
    }
}
