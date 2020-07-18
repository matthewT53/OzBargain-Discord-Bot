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
            int rate = bot.PostDelay / 1000;

            var embed = new EmbedBuilder();
            embed.WithTitle("The bot will post a new item every:  ");
            embed.WithDescription(rate.ToString() + " seconds.");
            embed.WithColor(Color.Orange);

            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }

        [Command("set_post_delay")]
        public async Task SetDelay([Remainder] int postDelay)
        {
            bot.PostDelay = postDelay * 1000;

            var embed = new EmbedBuilder();
            embed.WithTitle("The bot will post every: ");
            embed.WithDescription(postDelay.ToString() + " seconds.");
            embed.WithColor(Color.Orange);

            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }
    }
}
