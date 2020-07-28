using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace DiscordScraperBot.Modules
{
    public class PreferenceConfig : ModuleBase<SocketCommandContext>
    {
        Preferences UserPreferences { get; set; }

        public PreferenceConfig(Preferences preferences)
        {
            UserPreferences = preferences;
        }

        [Command("add_category")]
        public async Task AddCategory([Remainder] string category)
        {
            UserPreferences.AddCategory(category);

            var embed = new EmbedBuilder();
            embed.WithTitle("Result: ");
            embed.WithDescription("Added " + category + " as a user preference.");
            embed.WithColor(Color.Orange);

            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }

        [Command("remove_category")]
        public async Task RemoveCategory([Remainder] string category)
        {
            UserPreferences.RemoveCategory(category);

            var embed = new EmbedBuilder();
            embed.WithTitle("Result: ");
            embed.WithDescription("Removed " + category + " as a user preference.");
            embed.WithColor(Color.Orange);

            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }
    }
}
