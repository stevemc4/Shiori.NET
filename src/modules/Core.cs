using System.Linq;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;

namespace Shiori.Modules
{
    [Name("Core")]
    [Summary("Core Shiori commands")]
    public class Core: ModuleBase<SocketCommandContext>
    {
        private CommandService commands;

        public Core(CommandService commands)
        {
            this.commands = commands;
        }

        [Command("ping")]
        [Summary("Ping the bot")]
        public async Task PingAsync()
        {
            await ReplyAsync("pong!");
        }

        [Command("echo")]
        [Summary("Echo back a text")]
        public async Task EchoAsync([Remainder] string message)
        {
            await ReplyAsync(message);   
        }

        [Command("help")]
        [Summary("Show this help message")]
        public async Task HelpAsync()
        {
            EmbedBuilder embedBuilder = new EmbedBuilder()
                .WithColor(new Color(0xF55875))
                .WithTitle("Help Page")
                .WithDescription("Get help on the modules by typing `=help <module>`. Below is the available modules");

            foreach (ModuleInfo module in commands.Modules)
            {
                embedBuilder.AddField($"`=help {module.Name.ToLower()}`", $"**{module.Name}**: {module.Summary}");
            }

            await ReplyAsync(embed: embedBuilder.Build());
        }

        [Command("help")]
        [Summary("Show this help message")]
        public async Task HelpAsync(string module)
        {
            ModuleInfo moduleInfo = commands.Modules.FirstOrDefault(x => x.Name.ToLower() == module.ToLower());
            EmbedBuilder embedBuilder = new EmbedBuilder()
                .WithColor(new Color(0xF55875))
                .WithTitle($"Help Page: {moduleInfo.Name}")
                .WithDescription(moduleInfo.Summary);

            foreach (CommandInfo command in moduleInfo.Commands)
            {
                embedBuilder.AddField($"={command.Name.ToLower()}", command.Summary);
            }

            await ReplyAsync(embed: embedBuilder.Build());
        }
    }
}
