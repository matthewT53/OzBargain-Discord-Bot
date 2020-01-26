using Discord;
using Discord.WebSocket;
using DiscordScraperBot.BotMessages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscordScraperBot.Discord
{
    public class Bot
    {
        DiscordSocketClient _client;
        CommandHandler _cmd_handler;

        public async Task StartAsync(InitializeCommandHandler init)
        {
            Console.Out.WriteLine("[+] Inside StartAsync(): ");
            if (Config.bot.token == "" || Config.bot.token == null) return;

            _client = init._client;
            _client.Log += LogMessageAsync;

            // Log in and start the bot. 
            await _client.LoginAsync(TokenType.Bot, Config.bot.token);
            await _client.StartAsync();
            
            // Create the services that the modules will require:
            _cmd_handler = new CommandHandler(init);
            
            await _cmd_handler.InitialiseAsync();
        }

        public async Task SendMessagesAsync(List<IBotMessage> messages)
        {

        }

        private async Task LogMessageAsync(LogMessage m)
        {
            Console.WriteLine(m.Message);
        }
    }
}
