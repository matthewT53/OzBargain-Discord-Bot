using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DiscordScraperBot.Modules;
using Microsoft.Extensions.DependencyInjection;

namespace DiscordScraperBot
{
    class CmdHandler
    {
        DiscordSocketClient     _client;
        CommandService          _commands;
        IServiceProvider        _services;

        public CmdHandler(DiscordSocketClient client)
        {
            _client = client;
            _commands = new CommandService();
            _services = BuildServiceProvider();
        }

        public async Task InitialiseAsync()
        {
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
            _client.MessageReceived += HandleCommandMessageAsync;
        }

        public IServiceProvider BuildServiceProvider() =>   new ServiceCollection()
                                                            .AddSingleton(_client)
                                                            .AddSingleton(_commands)
                                                            // You can pass in an instance of the desired type
                                                            // .AddSingleton(new ScraperManager())
                                                            // ...or by using the generic method.
                                                            //
                                                            // The benefit of using the generic method is that 
                                                            // ASP.NET DI will attempt to inject the required
                                                            // dependencies that are specified under the constructor 
                                                            // for us.
                                                            .AddSingleton<ScraperManager>()
                                                            .BuildServiceProvider();

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
                var result = await _commands.ExecuteAsync(context, argPos, _services);
                if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
                {
                    Console.WriteLine(result.ErrorReason);
                }
            }
        }
    }
}
