using Discord;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordScraperBot.BotMessages
{
    class ProductMessage : IBotMessage 
    {
        public Embed GetEmbed()
        {
            Embed e = new EmbedBuilder().Build();
            return e;
        }
    }
}
