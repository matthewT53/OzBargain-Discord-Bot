using Discord.WebSocket;
using DiscordScraperBot.BotMessages;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordScraperBot.Discord
{
    class Endpoint
    {
        DiscordSocketClient _client;
        public Endpoint(DiscordSocketClient client)
        {
            _client = client;
        }

        void Dispatch(List<IBotMessage> messages)
        {
            
        }
    }
}
