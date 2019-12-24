using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordScraperBot
{
    class DiscordBot
    {
        DiscordSocketClient _client;
        CmdHandler _cmd_handler;
        ScraperManager _scraper_manager;
        Thread _scraper_thread;
        InitializeCmdHandler _init;

        static void Main(string[] args)
        => new DiscordBot().StartAsync().GetAwaiter().GetResult();

        public async Task StartAsync()
        {
            Console.Out.WriteLine("[+] Inside StartAsync(): ");
            if (Config.bot.token == "" || Config.bot.token == null) return;

            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Verbose
            });

            _client.Log += LogMessageAsync;

            // Log in and start the bot. 
            await _client.LoginAsync(TokenType.Bot, Config.bot.token);
            await _client.StartAsync();

            // Intitialize and start the scrapers.
            Console.WriteLine("[+] Starting scrapers.");
            _scraper_manager = new ScraperManager();
            _scraper_thread = new Thread(_scraper_manager.StartScraping);

            // Create the services that the modules will require:
            _init = new InitializeCmdHandler(ref _scraper_manager);
            _cmd_handler = new CmdHandler( ref _init);
            
            await _cmd_handler.InitialiseAsync();

            _scraper_manager.Initialize();
            _scraper_thread.Start();

            // Wait until the operation finishes.
            await Task.Delay(-1); 
        }

        private async Task LogMessageAsync(LogMessage m)
        {
            Console.WriteLine(m.Message);
        }
    }
}
