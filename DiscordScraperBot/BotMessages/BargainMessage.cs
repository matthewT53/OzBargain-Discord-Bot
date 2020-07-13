using Discord;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace DiscordScraperBot.BotMessages
{
    public class BargainMessage : IBotMessage, IEquatable<IBotMessage>
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

        public override int GetHashCode()
        {
            return  Name.GetHashCode() + 
                    Price.GetHashCode() + 
                    ExternalUrl.GetHashCode() + 
                    ImageUrl.GetHashCode();
        }

        public bool Equals(BargainMessage message)
        {
            return this.Name == message.Name &&
                    this.Price == message.Price &&
                    this.ExternalUrl == message.ExternalUrl &&
                    this.ImageUrl == message.ImageUrl;
        }

        public bool Equals([AllowNull] IBotMessage other)
        {
            return  other != null && 
                    other is BargainMessage && 
                    Equals((BargainMessage)other);
        }
    }
}
