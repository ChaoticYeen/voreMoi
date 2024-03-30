
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DSharpPlus;
using System.Reflection;
using System.Configuration;
using System.Collections.Specialized;
using VoreMoi.modules;
using DSharpPlus.CommandsNext;
using System.Data;
using DSharpPlus.SlashCommands;

namespace VoreMoi
{
    class Program
    {

        static async Task Main(string[] args)
        {
            // Utilisez le service comme un singleton
            var mySqlService = MySqlService.Instance;
            var discord = new DiscordClient(new DiscordConfiguration()
            {
                Token = ConfigurationManager.AppSettings["token"],
                TokenType = DSharpPlus.TokenType.Bot,
                Intents = DiscordIntents.AllUnprivileged | DiscordIntents.MessageContents
            });

            MessageService.getInstance(discord);

            var slash = discord.UseSlashCommands();

            //preparing command use
            var commands = discord.UseCommandsNext(new CommandsNextConfiguration()
            {
                StringPrefixes = new[] { "!" }
            });

            commands.RegisterCommands<CommandModule>();

            slash.RegisterCommands<ProfileCommand>();
            slash.RegisterCommands<VoreCommand>();
            slash.RegisterCommands<StoryCommand>();

            await discord.ConnectAsync();
            await Task.Delay(-1);
        }
    }
}