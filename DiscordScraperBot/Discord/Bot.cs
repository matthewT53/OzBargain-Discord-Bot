using Discord;
using Discord.WebSocket;
using DiscordScraperBot.BotMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordScraperBot.Discord
{
    public class Bot
    {
        DiscordSocketClient Client;
        CommandHandler CmdHandler;
        Preferences UserPreferences;
        FilterMessage Filter;
        public bool IsReady { get; private set; } = false;
        public int PostDelay { get; set; }
        public DateTime StartTime { get; private set;  }

        int DefaultPostDelay = 5000;

        public Bot()
        {
            this.PostDelay = (Config.bot.postDelay != 0) ? Config.bot.postDelay * 1000 : DefaultPostDelay;
        }

        public async Task StartAsync(InitializeCommandHandler init)
        {
            if (Config.bot.token == "" || Config.bot.token == null) return;

            Client = init.Client;
            Client.Log += LogMessageAsync;
            Client.Ready += ReadyEventAsync;

            UserPreferences = init.UserPreferences;
            Filter = new FilterMessage(UserPreferences);

            // Log in and start the bot. 
            await Client.LoginAsync(TokenType.Bot, Config.bot.token);
            await Client.StartAsync();
            
            // Create the services that the modules will require:
            CmdHandler = new CommandHandler(init);
            
            await CmdHandler.InitialiseAsync();
        }

        public async Task SendToChannelAsync(List<IBotMessage> messages)
        {
            if (IsReady)
            {
                var channel = Client.GetChannel(Config.bot.bargainChannelID) as IMessageChannel;
                foreach (IBotMessage message in messages)
                {
                    Console.WriteLine("[+] Considering message: " + message.Name);
                    if (Filter.IsDesirable(message))
                    {
                        await channel.SendMessageAsync("", false, message.GetEmbed());
                        Thread.Sleep(PostDelay);
                    }
                }
            }
        }

        private Task ReadyEventAsync()
        {
            Console.WriteLine("[+] Bot is ready!");
            IsReady = true;
            StartTime = DateTime.Now;
            return Task.CompletedTask;
        }

        private async Task LogMessageAsync(LogMessage m)
        {
            Console.WriteLine(m.Message);
            Logger.GetInstance().realLogger.Info(m.Message);
        }
    }
}
