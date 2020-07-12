using Discord;
using Discord.WebSocket;
using DiscordScraperBot.BotMessages;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordScraperBot.Discord
{
    public class Bot
    {
        DiscordSocketClient Client;
        CommandHandler CmdHandler;
        bool IsReady = false;
        int PostDelay = 5000;

        public async Task StartAsync(InitializeCommandHandler init)
        {
            Console.Out.WriteLine("[+] Inside StartAsync(): ");
            if (Config.bot.token == "" || Config.bot.token == null) return;

            Client = init._client;
            Client.Log += LogMessageAsync;
            Client.Ready += ReadyEventAsync;

            // Log in and start the bot. 
            await Client.LoginAsync(TokenType.Bot, Config.bot.token);
            await Client.StartAsync();
            
            // Create the services that the modules will require:
            CmdHandler = new CommandHandler(init);
            
            await CmdHandler.InitialiseAsync();
        }

        /***
         * This method will send the results acquired from a scraper to a specified channel.
         */
        public async Task SendToChannelAsync(List<IBotMessage> messages)
        {
            Console.WriteLine("Channel ID: " + Config.bot.bargainChannelID);
            
            if (IsReady)
            {
                var channel = Client.GetChannel(Config.bot.bargainChannelID) as IMessageChannel;

                foreach (IBotMessage message in messages)
                {
                    await channel.SendMessageAsync("", false, message.GetEmbed());
                    Thread.Sleep(PostDelay);
                }
            }
        }

        public void SetPostDelay(int newDelay)
        {
            PostDelay = newDelay;
        }

        public int GetPostDelay()
        {
            return PostDelay;
        }

        private Task ReadyEventAsync()
        {
            Console.WriteLine("[+] Bot is connected!");
            IsReady = true;
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
