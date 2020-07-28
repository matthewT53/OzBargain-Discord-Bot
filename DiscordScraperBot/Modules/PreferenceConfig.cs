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

        /***
         * This command will add a search filter into the user's preferences which will 
         * inform the bot about which products to display to the user.
         */
        [Command("add_filter")]
        public async Task AddFilter([Remainder] string category)
        {
            bool result = UserPreferences.AddCategory(category.ToLower());
            string message = (result) 
                ? "The category " + category + " added as a user preference." 
                : "Failed to add category!";

            var embed = new EmbedBuilder();
            embed.WithTitle("Adding category: ");
            embed.WithDescription(message);
            embed.WithColor(Color.Orange);

            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }

        /***
        * This command will remove a search filter from the user's preferences which will 
        * inform the bot about which products to display to the user.
        */
        [Command("remove_filter")]
        public async Task RemoveFilter([Remainder] string category)
        {
            bool result = UserPreferences.RemoveCategory(category.ToLower());
            string message = (result) 
                ? "The category " + category + " user preference has been removed." 
                : "Failed to remove category!";

            var embed = new EmbedBuilder();
            embed.WithTitle("Removing category: ");
            embed.WithDescription(message);
            embed.WithColor(Color.Orange);

            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }
    }
}
