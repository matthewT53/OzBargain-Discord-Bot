using Discord;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace DiscordScraperBot
{
    class Bot
    {
        DiscordSocketClient _client;
        CommandHandler _cmd_handler;

        public async Task StartAsync(InitializeCmdHandler init)
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

            // Wait until the operation finishes.
            await Task.Delay(-1); 
        }

        public async Task SendMessage()
        {

        }

        private async Task LogMessageAsync(LogMessage m)
        {
            Console.WriteLine(m.Message);
        }
    }
}
