using Discord;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace DiscordScraperBot.BotMessages
{
    public abstract class IBotMessage : IEquatable<IBotMessage>
    {
        public string Name { get; protected set; }
        public string Price { get; protected set; }
        public string ExternalUrl { get;  protected set; }
        public string ImageUrl { get; protected set; }

        /***
         * Uses the information acquired from scarping to create an Embed object that is presentable to discord.
         */
        public abstract Embed GetEmbed();

        public override int GetHashCode()
        {
            return Name.GetHashCode() +
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
            return other != null &&
                    other is BargainMessage &&
                    Equals((BargainMessage)other);
        }
    }


}
