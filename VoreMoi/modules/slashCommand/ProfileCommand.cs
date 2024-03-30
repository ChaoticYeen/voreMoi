using Discord;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VoreMoi.modules
{
    internal class ProfileCommand : ApplicationCommandModule
    {
        /*
         * add a new profile
         */
        [SlashCommand("new", "Profile management commands")]
        public async Task AddProfileComand(InteractionContext ctx, [Option("name", "name of the profile")] string name)
        {
            string query = "INSERT INTO profile (discord_id, name, level, exp, health, max_health, vore_count, vored_count, agility, strength, resist) values(" + ctx.User.Id + ", \"" + name + "\", " + 1 + ","+0+","+100+","+100+","+0+","+0+","+0+","+0+","+0+")";
            string checkQuery = "Select * from profile where discord_id = "+ ctx.User.Id + " AND name = \"" + name + "\"";
            if (name != null && name.Length > 0 && MySqlService.Instance.ExecuteQuery(checkQuery).Rows.Count == 0)
            {
                DataTable result = MySqlService.Instance.ExecuteQuery(query);
                await ctx.CreateResponseAsync("operation validate");
            }
            await ctx.CreateResponseAsync("operation canceled. Verify you set a name or you don't already have an profile with the same name");
        }

        [SlashCommand("desc", "set profil description")]
        public async Task EditProfileDesc(InteractionContext ctx, [Option("name", "name of the profile")] string name, [Option("param", "name of the profile")] string param)
        {
            if (name != "" && param != "")
            {
                DataTable result = MySqlService.Instance.ExecuteQuery("SELECT * from profile WHERE name = \"" + name + "\" AND discord_id = \"" + ctx.User.Id + "\"");
                if (result.Rows.Count == 1)
                {
                    string query = "UPDATE profile SET description=\"" + param + "\" WHERE name = \"" + name + "\" AND discord_id = \"" + ctx.User.Id + "\"";
                    DataTable update = MySqlService.Instance.ExecuteQuery(query);
                    await ctx.CreateResponseAsync(query);
                }
            }
            await ctx.CreateResponseAsync("something gone wrong, you miss an argument or profile does not exist");
        }

        [SlashCommand("avatar", "set avatar url")]
        public async Task EditProfileAvatar(InteractionContext ctx, [Option("name", "name of the profile")] string name, [Option("param", "url")] string param)
        {
            if (name != "" && param != "")
            {
                DataTable result = MySqlService.Instance.ExecuteQuery("SELECT * from profile WHERE name = \"" + name + "\" AND discord_id = \"" + ctx.User.Id + "\"");
                if (result.Rows.Count == 1)
                {
                    string query = "UPDATE profile SET avatar=\"" + param + "\" WHERE name = \"" + name + "\" AND discord_id = \"" + ctx.User.Id + "\"";
                    DataTable update = MySqlService.Instance.ExecuteQuery(query);
                    await ctx.CreateResponseAsync("Avatar changé");
                }
            }
            await ctx.CreateResponseAsync(param);
        }

        /**
         * Get the profile and show it
         * 
         **/
        [SlashCommand("getProfile", "get profile")]
        public async Task GetProfileAvatar(InteractionContext ctx, [Option("name", "name of the profile")] string name)
        {
            if (name != "")
            {
                DataTable result = MySqlService.Instance.ExecuteQuery("SELECT * from profile WHERE name = \"" + name + "\" AND discord_id = \"" + ctx.User.Id + "\"");
                if (result.Rows.Count == 1)
                {
                    string query = "SELECT * FROM profile WHERE name = \"" + name + "\" AND discord_id = \"" + ctx.User.Id + "\"";
                    DataTable profile = MySqlService.Instance.ExecuteQuery(query);
                    DiscordEmbedBuilder embedBuilder = new DiscordEmbedBuilder();

                    //header block
                    embedBuilder.WithTitle((string)profile.Rows[0]["name"]);
                    embedBuilder.WithDescription(prepareDescriptor("description", profile));
                    embedBuilder.WithThumbnail(prepareDescriptor("avatar", profile));
                    //First line
                    embedBuilder.AddField("Role", prepareDescriptor("role", profile), true);
                    embedBuilder.AddField("Size", prepareDescriptor("size", profile), true);
                    embedBuilder.AddField("gender", prepareDescriptor("gender", profile), true);
                    //second line
                    embedBuilder.AddField("Health", profile.Rows[0]["health"] + "/"+ profile.Rows[0]["max_health"], false);
                    //thrid line
                    embedBuilder.AddField("Lvl", profile.Rows[0]["level"]+"", true);
                    embedBuilder.AddField("Exp", profile.Rows[0]["exp"]+"", true);
                    //Separator
                    embedBuilder.AddField("Stat", "Statistiques", false);
                    //last line
                    embedBuilder.AddField("agility", profile.Rows[0]["agility"] + "/20", true);
                    embedBuilder.AddField("Strength", profile.Rows[0]["strength"] + "/20", true);
                    embedBuilder.AddField("Resilience", profile.Rows[0]["resist"]+ "/20", true);

                    await ctx.CreateResponseAsync(embedBuilder);
                }
            }
            await ctx.CreateResponseAsync("impossible de trouver l'avatar");
        }

        private string prepareDescriptor(string value, DataTable profile)
        {
            try
            {
                return (string)profile.Rows[0][value];
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("impossible to load value for : " + value);
                return "not set";
            }
        }

        /**
         * Profile configuration step 2
         **/
        [SlashCommand("configureProfile", "avatar configuration")]
        public async Task ConfigureProfile(InteractionContext ctx, [Option("profile", "name of the profile")] string name)
        {
            //prepare DM channel
            DiscordDmChannel DMChannel = await ctx.Member.CreateDmChannelAsync();
            await ctx.CreateResponseAsync("next step look at your DM");

            //buiuld the embed message
            bool isInteraction = true;

            DiscordMessageBuilder builder = new DiscordMessageBuilder();
            DiscordEmbedBuilder embedBuilder = new DiscordEmbedBuilder();
            embedBuilder.WithTitle("Profile creation");
            embedBuilder.WithDescription("follow commands to edit your profile "+name+"\n"
                +"/size : edit the size in cm \n"
                +"/avatar : edit your profile avatar");
            builder.AddEmbed(embedBuilder).SendAsync(DMChannel);
            ctx.Client.MessageCreated += async (s, e) =>
            {
                if (!e.Message.Author.IsBot)
                {
                    string[] message = e.Message.Content.Split(" ");
                    switch (message[0])
                    {
                        case "/size":
                            this.changeSize(message[1], name, ctx.Member.Id, DMChannel);
                            break;
                    }
                    
                }
                /*
                await e.Interaction.CreateResponseAsync(DSharpPlus.InteractionResponseType.DeferredMessageUpdate);
                // Do things.. //
                startConfiguration(s, e); */
            };
        }

        /*
         * change size. param size must be a number
         */
        private void changeSize(string size, string name, ulong discordId, DiscordDmChannel dmChannel)
        {
            int number;

            bool isParsable = Int32.TryParse(size, out number);
            if (isParsable)
            {
                updateValue("size", number, name, discordId);
                dmChannel.SendMessageAsync("new size set to "+number+"cm");
            }
            else
            {
                dmChannel.SendMessageAsync("Can't convert the entry, please send a number");
            }
        }

        /*
         * prepare and send update request on profile table
         * @param : column - column to update
         * @param : value - value to set
         * @param : name - name of the profile
         * @param ; discordId - discord id of the use
         */
        private void updateValue(string column, string value, string name, ulong discordId)
        {
            string query = "UPDATE profile " +
                "SET  " + column + " = " + value + " " +
                "WHERE name = \"" + name + "\" AND discord_id = \"" + discordId + "\"";
            MySqlService.Instance.ExecuteQuery(query);
        }
        private void updateValue(string column, int value, string name, ulong discordId)
        {
            string query = "UPDATE profile " +
                "SET  " + column + " = " + value + " "+
                "WHERE name = \"" + name + "\" AND discord_id = \"" + discordId + "\"";
            MySqlService.Instance.ExecuteQuery(query);
        }
    }
}
