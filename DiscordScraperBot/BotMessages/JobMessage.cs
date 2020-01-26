using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordScraperBot.BotMessages
{
    public class JobMessage : IBotMessage
    {
        public Embed GetEmbed()
        {
            Embed e = new EmbedBuilder().Build();
            return e;
        }
    }
}
