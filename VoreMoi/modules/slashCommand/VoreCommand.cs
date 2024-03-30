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
    internal class VoreCommand : ApplicationCommandModule
    {
        [SlashCommand("devour", "Profile management commands")]
        public async Task AddProfileComand(InteractionContext ctx, [Option("name", "name of the profile")] string name)
        {
            String query = "";
            query = "INSERT INTO profile (discord_id, name_profile) values(" + ctx.User.Id+", \""+name+ "\")";
            if(query != "")
            {
                DataTable result = MySqlService.Instance.ExecuteQuery(query);
                await ctx.CreateResponseAsync((string)result.Rows[0]["discord_id"]);
                await ctx.CreateResponseAsync("operation validéz");
            }
        }
    }
}
