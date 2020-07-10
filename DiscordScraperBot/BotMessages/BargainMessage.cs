using Discord;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordScraperBot.BotMessages
{
    public class BargainMessage : IBotMessage
    {
        string name { get; set; }
        string price { get; set; }
        string externalUrl { get; set; }
        string imageUrl { get; set; }

        public BargainMessage(string name, string price, string externalUrl, string imageUrl)
        {
            this.name = name;
            this.price = price;
            this.externalUrl = externalUrl;
            this.imageUrl = imageUrl;
        }

        public Embed GetEmbed()
        {
            var embed = new EmbedBuilder();
            embed.Title = name;
            embed.Color = Color.Green;
            embed.AddField("Price: ", string.IsNullOrEmpty(price) ? "No Price found" : price);
            embed.AddField("Source: ", string.IsNullOrEmpty(externalUrl) ? "No source found" : externalUrl);
            embed.ImageUrl = imageUrl;

            return embed.Build();
        }
    }
}
