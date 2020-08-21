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
        Preferences UserPreferences;
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
            if (IsReady)
            {
                var channel = Client.GetChannel(Config.bot.bargainChannelID) as IMessageChannel;
                foreach (IBotMessage message in messages)
                {
                    Console.WriteLine("[+] Considering message: " + message.Name);
                    if (isDesirable(message))
                    {
                        await channel.SendMessageAsync("", false, message.GetEmbed());
                        Thread.Sleep(PostDelay);
                    }
                }
            }
        }

        /***
         * Determines if a message is desirable to the user. 
         * @Returns: 
         *  true if the message should be posted because it matches a filter applied by the user.
         *  false otherwise.
         */
        private bool isDesirable(IBotMessage message)
        {
            bool desirable = false;
            // No filters have been created by the user so we accept the message.
            if (UserPreferences.Count() == 0)
            {
                Console.WriteLine("[+] No filters applied so we take all messages!");
                return true;
            }

            else
            {
                foreach (string category in message.Categories)
                {
                    Console.WriteLine("[+] Considering category: " + category);
                    UserPreference preference;

                    if (UserPreferences.FindUserPreferenceFromCache(category, out preference))
                    {
                        desirable = true;
                        break;
                    }
                }

                if (!desirable)
                {
                    // Consider the title of the message too!
                    string [] titleHints = message.Name.Split(' ');
                    foreach (string hint in titleHints)
                    {
                        Console.WriteLine("[+] Hint: " + hint);
                        UserPreference preference;
                        if (UserPreferences.FindUserPreferenceFromCache(hint, out preference))
                        {
                            desirable = true;
                            break;
                        }
                    }
                }

            }

            return desirable;
        }

        private Task ReadyEventAsync()
        {
            Console.WriteLine("[+] Bot is ready!");
            IsReady = true;
            StartTime = DateTime.Now;
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
