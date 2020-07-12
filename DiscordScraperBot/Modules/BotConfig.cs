using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using DiscordScraperBot.Discord;

namespace DiscordScraperBot.Modules
{
    public class BotConfig : ModuleBase<SocketCommandContext>
    {
        private Bot bot;

        public BotConfig(Bot bot)
        {
            this.bot = bot;
        }

        [Command("show_post_delay")]
        public async Task ShowDelay()
        {
            int rate = bot.GetPostDelay();

            var embed = new EmbedBuilder();
            embed.WithTitle("Post delay: ");
            embed.WithDescription(rate.ToString());
            embed.WithColor(new Color(10, 98, 234));

            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }

        [Command("set_post_delay")]
        public async Task SetDelay([Remainder] int postDelay)
        {
            bot.SetPostDelay(postDelay);

            var embed = new EmbedBuilder();
            embed.WithTitle("Changed post delay to: ");
            embed.WithDescription(postDelay.ToString());
            embed.WithColor(new Color(10, 98, 234));

            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }
    }
}
