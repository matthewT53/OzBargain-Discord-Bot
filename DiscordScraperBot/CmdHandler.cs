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
            _client.MessageReceived += HandleCommandMessageAsync;
        }

        private async Task HandleCommandMessageAsync(SocketMessage s)
        {
            var msg = s as SocketUserMessage;
            if (msg == null) 
                return;

            var context = new SocketCommandContext(_client, msg);
            int argPos = 0;

            // Handle the event where the user enters a command or mentions the bot.
            // _client.CurrentUser is the bot.
            if (msg.HasStringPrefix(Config.bot.cmdPrefix, ref argPos) 
                || msg.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {
                var result = await _service.ExecuteAsync(context, argPos, null);
                if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
                {
                    Console.WriteLine(result.ErrorReason);
                }
            }
        }
    }
}
