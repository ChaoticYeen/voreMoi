using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoreMoi.modules
{
    internal class AvatarCommand : BaseCommandModule
    {
        [Command("avatar")]
        public async Task Avatar(CommandContext ctx)
        {
            // Exemple : exécution d'une requête
            String avatar = ctx.Message.Attachments[0].ToString();
            await ctx.RespondAsync("test");
        }
    }
}
