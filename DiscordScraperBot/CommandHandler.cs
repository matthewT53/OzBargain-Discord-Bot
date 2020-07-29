using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using DiscordScraperBot.Discord;
using DiscordScraperBot.Scrapers;

namespace DiscordScraperBot
{
    public class InitializeCommandHandler
    {
        public DiscordSocketClient Client { get; set; }
        public CommandService Commands { get; set; }
        public IServiceProvider Services { get; set; }

        ScraperManager ScrapeManager { get; set; }
        Bot DiscordBot { get; set; }
        Preferences UserPreferences { get; set; }

        public InitializeCommandHandler(ScraperManager scraperManager, Bot bot, Preferences preferences)
        {
            /*
             * Create a DiscordSocketClient object which will allow us to communicate with our BOT 
             * through the Discord API.
             */
            Client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Verbose
            });

            Commands = new CommandService();

            ScrapeManager = scraperManager;
            DiscordBot = bot;
            UserPreferences = preferences;

            Services = BuildServiceProvider();
        }

        private IServiceProvider BuildServiceProvider() => new ServiceCollection()
            .AddSingleton(Client)
            .AddSingleton(Commands)
            // You can pass in an instance of the desired type
            // ...or by using the generic method.
            //
            // The benefit of using the generic method is that 
            // ASP.NET DI will attempt to inject the required
            // dependencies that are specified under the constructor 
            // for us.
            .AddSingleton(ScrapeManager)
            .AddSingleton(DiscordBot)
            .AddSingleton(UserPreferences)
            .BuildServiceProvider();
    }

    public class CommandHandler
    {
        private DiscordSocketClient Client;
        private CommandService Commands;
        private IServiceProvider Services;

        public CommandHandler(InitializeCommandHandler init)
        {
            if (init == null)
            {
                throw new ArgumentNullException();
            }

            Client     = init.Client;
            Commands   = init.Commands;
            Services   = init.Services;
        }

        public async Task InitialiseAsync()
        {
            await Commands.AddModulesAsync(Assembly.GetEntryAssembly(), Services);
            Client.MessageReceived += HandleCommandMessageAsync;
        }

        private async Task HandleCommandMessageAsync(SocketMessage s)
        {
            var msg = s as SocketUserMessage;
            if (msg == null)
                return;

            var context = new SocketCommandContext(Client, msg);
            int argPos = 0;

            // Handle the event where the user enters a command or mentions the bot.
            // _client.CurrentUser is the bot.
            if (msg.HasStringPrefix(Config.bot.commandPrefix, ref argPos)
                || msg.HasMentionPrefix(Client.CurrentUser, ref argPos))
            {
                var result = await Commands.ExecuteAsync(context, argPos, Services);
                if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
                {
                    Console.WriteLine(result.ErrorReason);
                }
            }
        }
    }
}
