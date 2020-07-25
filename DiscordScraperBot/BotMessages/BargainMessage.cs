using Discord;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace DiscordScraperBot.BotMessages
{
    public class BargainMessage : IBotMessage
    {
        public string Category { get; private set; }

        public BargainMessage(string name, string price, string externalUrl, string imageUrl, string category)
        {
            Name = name;
            Price = price;
            ExternalUrl = externalUrl;
            ImageUrl = imageUrl;
            Category = category;
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
