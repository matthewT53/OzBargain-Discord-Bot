using Discord;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordScraperBot.BotMessages
{
    public class BargainMessage : IBotMessage
    {
        string name;
        string price;
        string externalUrl;
        string imageUrl;

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
            return embed.Build();
        }
    }
}
