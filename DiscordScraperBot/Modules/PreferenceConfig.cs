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

        [Command("add_filter_price")]
        public async Task AddFilterPrice(string category, double minPrice, [Remainder] double maxPrice)
        {
            Tuple<double, double> price = new Tuple<double, double>(minPrice, maxPrice);
            bool result = UserPreferences.AddCategory(category.ToLower(), price);
            string message = (result)
                ? "The category " + category + " with price: " + "(" + minPrice + "," + maxPrice + ") has been added!"
                : "Failed to add category with price!";

            var embed = new EmbedBuilder();
            embed.WithTitle("Adding price filter: ");
            embed.WithDescription(message);
            embed.WithColor(Color.Orange);

            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }

        [Command("remove_filter_price")]
        public async Task RemoveFilterPrice([Remainder] string category)
        {
            bool result = UserPreferences.RemovePriceRange(category.ToLower());
            string message = (result)
                ? "The price filter has been removed for category: " + category
                : "Failed to remove price filter!";

            var embed = new EmbedBuilder();
            embed.WithTitle("Removing price filter: ");
            embed.WithDescription(message);
            embed.WithColor(Color.Orange);

            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }

        [Command("update_filter_price")]
        public async Task UpdateFilterPrice(string category, double minPrice, [Remainder] double maxPrice)
        {
            Tuple<double, double> price = new Tuple<double, double>(minPrice, maxPrice);
            bool result = UserPreferences.AddPriceRange(category.ToLower(), price);
            string message = (result)
                ? "The price range of " + category + " has been updated to: " + "(" + minPrice + "," + maxPrice + ")"
                : "Failed to update category price!";

            var embed = new EmbedBuilder();
            embed.WithTitle("Updating price filter: ");
            embed.WithDescription(message);
            embed.WithColor(Color.Orange);

            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }

        [Command("show_filters")]
        public async Task ShowFilters()
        {
            var embed = new EmbedBuilder();
            embed.WithTitle("Filters: ");
            embed.WithColor(Color.Orange);

            List<UserPreference> preferences = UserPreferences.GetUserPreferences();
            if (preferences.Count != 0)
            {
                foreach (UserPreference preference in preferences)
                {
                    embed.AddField(
                        preference._category,
                        "(Min price: " + preference._minPrice + ", Max price: " + preference._maxPrice + ")"
                    );
                }
            }
            
            else
            {
                embed.WithDescription("No filters found!");
            }

            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }
    }
}
