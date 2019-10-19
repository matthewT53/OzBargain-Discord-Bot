using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DiscordScraperBot
{
    class CmdHandler
    {
        DiscordSocketClient _client;
        CommandService _service;

        public async Task InitialiseAsync(DiscordSocketClient client)
        {
            _client = client;
            _service = new CommandService();
            await _service.AddModulesAsync(Assembly.GetEntryAssembly(), null);
            _client.MessageReceived += HandleCommandMessage;
        }

        private async Task HandleCommandMessage(SocketMessage s)
        {
            var msg = s as SocketUserMessage;
            if (msg == null) 
                return;

            var context = new SocketCommandContext(_client, msg);
        }
    }
}
