using Discord;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace DiscordScraperBot
{
    class Program
    {
        DiscordSocketClient _client;
        CmdHandler _cmd_handler;
        static void Main(string[] args)
        => new Program().StartAsync().GetAwaiter().GetResult();

        public async Task StartAsync()
        {
            if (Config.bot.token == "" || Config.bot.token == null) return;

            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Verbose
            });

            _client.Log += LogMessageAsync;
        }

        private async Task LogMessageAsync(LogMessage m)
        {
            Console.WriteLine(m.Message);
        }
    }
}
