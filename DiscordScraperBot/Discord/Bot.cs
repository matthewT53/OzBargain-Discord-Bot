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
        CommandHandler _cmdHandler;
        Boolean _isReady;

        public async Task StartAsync(InitializeCommandHandler init)
        {
            Console.Out.WriteLine("[+] Inside StartAsync(): ");
            if (Config.bot.token == "" || Config.bot.token == null) return;

            _client = init._client;
            _client.Log += LogMessageAsync;
            _client.Ready += ReadyEventAsync;

            // Log in and start the bot. 
            await _client.LoginAsync(TokenType.Bot, Config.bot.token);
            await _client.StartAsync();
            
            // Create the services that the modules will require:
            _cmdHandler = new CommandHandler(init);
            
            await _cmdHandler.InitialiseAsync();
        }

        /***
         * This method will send the results acquired from a scraper to a specified channel.
         */
        public async Task SendToChannelAsync(List<IBotMessage> messages)
        {
            Console.WriteLine("Channel ID: " + Config.bot.bargainChannelID);
            
            if ( _isReady )
            {
                var channel = _client.GetChannel(Config.bot.bargainChannelID) as IMessageChannel;


                var embed = new EmbedBuilder();

                embed.WithTitle("Test message sent by the bot ");
                embed.WithDescription("info");
                embed.WithColor(new Color(10, 98, 234));

                await channel.SendMessageAsync("", false, embed.Build());
            }
        }

        private Task ReadyEventAsync()
        {
            Console.WriteLine("[+] Bot is connected! ");
            _isReady = true;
            return Task.CompletedTask;
        }

        /***
         * Logs a message to a file.
         */
        private async Task LogMessageAsync(LogMessage m)
        {
            Console.WriteLine(m.Message);
        }
    }
}
