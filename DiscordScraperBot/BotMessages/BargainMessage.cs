using Discord;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordScraperBot.BotMessages
{
    public class BargainMessage : IBotMessage
    {
        string Name { get; set; }
        string Price { get; set; }
        string ExternalUrl { get; set; }
        string ImageUrl { get; set; }

        public BargainMessage(string name, string price, string externalUrl, string imageUrl)
        {
            this.Name = name;
            this.Price = price;
            this.ExternalUrl = externalUrl;
            this.ImageUrl = imageUrl;
        }

        public Embed GetEmbed()
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
