using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBOT.Core.Commands
{
    public class GreetingCommand:ModuleBase 
    {
        [Command("hello") ]
        public async Task Say()
        {

            await Context.Channel.SendMessageAsync("Hi, how`s going?");
        }
        [Command("userinfo")]
        public async Task UserInfo( IUser user = null)
        {
            var userInfo = user ?? Context.Client.CurrentUser;
            await ReplyAsync($"{userInfo.Username}#{userInfo.Discriminator}");
        }
    }
}
