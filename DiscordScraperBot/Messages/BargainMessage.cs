using Discord;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace DiscordScraperBot.BotMessages
{
    public class BargainMessage : IBotMessage
    {
        public List<string> Categories { get; private set; }

        public BargainMessage(string name, string price, string externalUrl, string imageUrl, string category)
        {
            Name = name;
            Price = price;
            ExternalUrl = externalUrl;
            ImageUrl = imageUrl;
            Categories = new List<string>();

            string[] extractedCategories = category.Split(' ');
            foreach (string bargainCategory in extractedCategories)
            {
                Categories.Add(bargainCategory);
            }
        }

        public override Embed GetEmbed()
        {
            var embed = new EmbedBuilder();
            embed.Title = Name;
            embed.Color = Color.Green;
            embed.AddField("Price: ", string.IsNullOrEmpty(Price) ? "No Price found" : Price);
            embed.AddField("Source: ", string.IsNullOrEmpty(ExternalUrl) ? "No source found" : ExternalUrl);
            embed.ImageUrl = ImageUrl;

            return embed.Build();
        }
    }
}
