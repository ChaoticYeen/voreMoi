using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoreMoi.modules
{
    internal class StoryCommand : ApplicationCommandModule
    {
        [SlashCommand("story", "start a story")]
        public async Task AddProfileComand(InteractionContext ctx, [Option("name", "name of the profile")] string name)
        {
            if (name != "")
            {
                DataTable result = MySqlService.Instance.ExecuteQuery("SELECT * from story");
                if (result.Rows.Count >= 1)
                {
                    var builder = new DiscordMessageBuilder();

                    DiscordEmbedBuilder embedBuilder = new DiscordEmbedBuilder();
                    embedBuilder.WithTitle("story available");
                    String description = "choose your story \n";
                    for (int i = 0; i<result.Rows.Count; i++)
                    {
                        description += i+" - "+ result.Rows[i]["title"]+ "\n";
                        builder.AddComponents(new DiscordButtonComponent(DSharpPlus.ButtonStyle.Primary, "choose a story", i.ToString(), false));
                    }
                    embedBuilder.WithDescription(description);
                    builder.AddEmbed(embedBuilder).SendAsync(ctx.Channel);

                    
                }
            } else
            {
                await ctx.CreateResponseAsync("no Story");
            }
        }
    }
}
