
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using DotNetEnv;

namespace TemplateBot
{
    class Program
    {
        static void Main(string[] args)
            => new Program().StartAsync().GetAwaiter().GetResult();
        private CommandHandler _handler;
        private DiscordSocketClient _client;
        ServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandler>()
                .AddSingleton<HttpClient>()
                .BuildServiceProvider();
        }
        public async Task StartAsync()
        {
            // Load environment variables from .env file
            Env.Load();
            string token = Environment.GetEnvironmentVariable("DISCORD_BOT_TOKEN");

            if (string.IsNullOrEmpty(token))
            {
                await Log("ERROR: DISCORD_BOT_TOKEN not found in .env file", ConsoleColor.Red);
                return;
            }

            using (ServiceProvider services = ConfigureServices())
            {
                await Log("Setting up the bot", ConsoleColor.Green);
                _client = services.GetRequiredService<DiscordSocketClient>();

                _handler = new CommandHandler(_client);
                await Log("Logging in...", ConsoleColor.Green);
                await _client.LoginAsync(TokenType.Bot, token);
                await Log("Connecting...", ConsoleColor.Green);
                await _client.StartAsync();
                _client.GuildAvailable += _client_GuildAvailable;

                await Task.Delay(-1);

            }

        }

        private async Task _client_GuildAvailable(SocketGuild arg)
        {

            await Log(arg.Name + " Connected!", ConsoleColor.Green);
        }
        public static async Task Log(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(DateTime.Now + " : " + message, color);
            Console.ResetColor();
        }

    }
}