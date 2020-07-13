using Discord;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordScraperBot.BotMessages
{
    public interface IBotMessage : IEquatable<IBotMessage>
    {
        public Embed GetEmbed();
    }
}
