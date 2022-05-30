using System;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace Shiori
{
    public class App
    {
        private DiscordSocketClient client;

        public static Task Main() {
            DotNetEnv.Env.TraversePath().Load();

            return new App().RunAsync();    
        } 

        public async Task RunAsync()
        {
            client = new DiscordSocketClient(); 

            client.Connected += OnLoggedIn;

            await client.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("DISCORD_TOKEN"));
            await client.StartAsync();

            await Task.Delay(-1);
        }

        private async Task OnLoggedIn()
        {
            CommandHandler handler = new CommandHandler(client, new CommandService());

            await client.SetActivityAsync(new Game("Your Heart", ActivityType.Listening));
            await handler.InitializeAsync();

            Console.WriteLine($"Logged in as {client.CurrentUser.Username}#{client.CurrentUser.Discriminator}");
        }
    }
}
