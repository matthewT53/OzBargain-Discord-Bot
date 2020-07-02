using Discord;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordScraperBot.BotMessages
{
    class BargainMessage : IBotMessage
    {
        public Embed GetEmbed()
        {
            var embed = new EmbedBuilder();
            return embed.Build();
        }
    }
}
