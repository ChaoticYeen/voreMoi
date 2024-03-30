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
    internal class CommandModule : BaseCommandModule
    {
        [Command("random")]
        public async Task RandomSentence(CommandContext ctx)
        {
            // Exemple : exécution d'une requête
            //await ctx.RespondAsync((string)result.Rows[0]["sentence"]);
        }
    }
}
