using DSharpPlus;
using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoreMoi.modules
{
    internal class MessageService
    {
        //lock default constructor
        private MessageService() { }
           
        //hold the instance
        private static MessageService _instance;
        //Lock for thread safe
        private static readonly object _lock = new object();

        public static MessageService getInstance(DiscordClient s) 
        { 
            if(_instance == null)
            {
                lock(_lock)
                {
                    if(_instance == null)
                    {
                        _instance = new MessageService();
                        _instance.Client = s;
                        s.MessageCreated += MessageCreateHandler;
                    }
                }
            }
            return _instance; 
        }

        public DiscordClient Client {  get; private set; }

        private static async Task MessageCreateHandler(DiscordClient s, MessageCreateEventArgs e)
        {
            if (e.Message.Content.ToLower().StartsWith("ping")) await e.Message.RespondAsync("pong!");
        }
    }
}
