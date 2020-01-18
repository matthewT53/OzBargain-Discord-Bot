using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Discord;

namespace DiscordScraperBot
{
    public class InitializeCommandHandler
    {
        public DiscordSocketClient _client { get; set; }
        public CommandService _commands { get; set; }
        public IServiceProvider _services { get; set; }

        public InitializeCommandHandler(ScraperManager scrape_manager)
        {
            Console.WriteLine("[+] IntializeCmdHandler: ");

            /*
             * Create a DiscordSocketClient object which will allow us to communicate with our BOT 
             * through the Discord API.
             */
            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Verbose
            });

            _commands = new CommandService();
            _services = BuildServiceProvider(scrape_manager);

            Console.WriteLine("[+] _client hashcode: " + _client.GetHashCode());
            Console.WriteLine("[+] _commands hashcode: " + _commands.GetHashCode());
            Console.WriteLine("[+] _services hashcode: " + _services.GetHashCode());
        }

        private IServiceProvider BuildServiceProvider(ScraperManager scrape_manager) => new ServiceCollection()
            .AddSingleton(_client)
            .AddSingleton(_commands)
            // You can pass in an instance of the desired type
            // ...or by using the generic method.
            //
            // The benefit of using the generic method is that 
            // ASP.NET DI will attempt to inject the required
            // dependencies that are specified under the constructor 
            // for us.
            .AddSingleton(scrape_manager)
            .BuildServiceProvider();
    }

    public class CommandHandler
    {
        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;

        public CommandHandler(InitializeCommandHandler init)
        {
            if (init == null)
            {
                throw new ArgumentNullException();
            }

            _client     = init._client;
            _commands   = init._commands;
            _services   = init._services;

            Console.WriteLine("[+] CmdHandler: ");
            Console.WriteLine("[+] _client hashcode: " + _client.GetHashCode());
            Console.WriteLine("[+] _commands hashcode: " + _commands.GetHashCode());
            Console.WriteLine("[+] _services hashcode: " + _services.GetHashCode());
        }

        public async Task InitialiseAsync()
        {
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
            _client.MessageReceived += HandleCommandMessageAsync;
        }

        private async Task HandleCommandMessageAsync(SocketMessage s)
        {
            Console.WriteLine("[+] Command message received: ");
            var msg = s as SocketUserMessage;
            if (msg == null)
                return;

            var context = new SocketCommandContext(_client, msg);
            int argPos = 0;

            // Handle the event where the user enters a command or mentions the bot.
            // _client.CurrentUser is the bot.
            if (msg.HasStringPrefix(Config.bot.commandPrefix, ref argPos)
                || msg.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {
                var result = await _commands.ExecuteAsync(context, argPos, _services);
                if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
                {
                    Console.WriteLine(result.ErrorReason);
                }
            }
        }
    }
}
