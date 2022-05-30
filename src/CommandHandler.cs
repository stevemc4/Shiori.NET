using System;
using System.Threading.Tasks;

using Discord.Commands;
using Discord.WebSocket;

namespace Shiori
{
    public class CommandHandler
    {
        private DiscordSocketClient client;
        private CommandService commands;

        public CommandHandler(DiscordSocketClient client, CommandService commands)
        {
            this.client = client;
            this.commands = commands;
        }

        public async Task InitializeAsync()
        {
            client.MessageReceived += OnMessageReceived;
            await commands.AddModulesAsync(
                System.Reflection.Assembly.GetEntryAssembly(),
                services: null
            );
        }

        private async Task OnMessageReceived(SocketMessage message)
        {
            SocketUserMessage msg = message as SocketUserMessage;
            if (msg == null) return;

            #if DEBUG
            int argumentPosition = 1;
            bool isMessageValid = msg.HasStringPrefix("t=", ref argumentPosition) || msg.HasMentionPrefix(client.CurrentUser, ref argumentPosition);
            #else
            int argumentPosition = 0;
            bool isMessageValid = msg.HasStringPrefix("=", ref argumentPosition) || msg.HasMentionPrefix(client.CurrentUser, ref argumentPosition);
            #endif

            if (!isMessageValid || msg.Author.IsBot) return;

            SocketCommandContext context = new SocketCommandContext(client, msg);

            await commands.ExecuteAsync(context, argumentPosition, null);
        }
    }
}
