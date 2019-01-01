using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBOT.Core.Commands
{ 
    [RequireOwner] 
    public class AdminsCommand : ModuleBase
    {
        [Group("clean")]
        public class CleanModule : ModuleBase
        { 
            [Command]
            public async Task Default(ulong count = 10) => Messages(count);
             
            [Command("messages")]
            public async Task Messages(ulong count = 10) {

                await Context.Channel.DeleteMessageAsync(count);
            }
        }
        [Group("user")]
        public class UserModule : ModuleBase
        {

            [Command("kick")]
            public async Task Kick(IGuildUser user)
            {
                await user.KickAsync();
            }
            public async Task Ban(IGuildUser user)
            {
                await user.BanAsync();
            }
            [Command("unban")]
            public async Task UnBan(IGuildUser user)
            {
                await Context.Guild.RemoveBanAsync(user);
            }
        }
    }
}
