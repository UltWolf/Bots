using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBOT.Core.Commands
{
    public class GreetingCommand:ModuleBase 
    {
        [Command("hello"), Alias("Привет","Хай","Здравствуйте","Hi") ]
        public async Task Say()
        { 
            await Context.Channel.SendMessageAsync("Hi, how`s going?");
        }
        [Command("userinfo"),Alias("Информация","Расскажи о")]
        public async Task UserInfo( IUser user = null)
        {
            var userInfo = user ?? Context.Client.CurrentUser; 
            await ReplyAsync($"{userInfo.Username}#{userInfo.Discriminator}");
        }
        [Command("embed"), Summary("Embed test command")]
        public async Task Embed([Remainder]string Input = "None")
        {
            EmbedBuilder Embed = new EmbedBuilder();
            Embed.WithAuthor("Test embed", Context.User.GetAvatarUrl());
            Embed.WithColor(40, 200, 150);
            Embed.WithFooter("The footer of the embed");
            Embed.WithDescription("This is a description of this method.");
            Embed.AddField("User input:", Input);
            await Context.Channel.SendMessageAsync("", false, Embed.Build());
        }
    }
}
