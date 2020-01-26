using Discord;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordScraperBot.BotMessages
{
    public class NewsMessage : IBotMessage
    {
        public Embed GetEmbed()
        {
            Embed e = new EmbedBuilder().Build();
            return e;
        }
    }
}
