using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Music_discord_bot
{
    class Program
    {
        private DiscordSocketClient client;
        private IServiceProvider services;
        public static void Main(string[] args)
               => new Program().MainAsync().GetAwaiter().GetResult();
        private CommandService command;
        public async Task MainAsync()
        {
            client = new DiscordSocketClient();
           
            command = new CommandService()  ;
      
           

            client.Log += Log;
            string token = "";

            string path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location).Replace(@"bin\Debug\netcoreapp2.1", @"Data\Token.txt");
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    token = sr.ReadToEnd();
                }

            }

            services = new ServiceCollection()
              .BuildServiceProvider();
            await InstallCommands();
            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();

            await Task.Delay(-1);
        }

       
        public async Task InstallCommands()
        {
            client.MessageReceived += MessageReceived;
            await command.AddModulesAsync(Assembly.GetEntryAssembly(), services);
        }
        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;

        }

        public async Task MessageReceived(SocketMessage messageParam)
        {
            var message = messageParam as SocketUserMessage;
            if (message == null) return;
            int argPos = 0;
            if (!(message.HasCharPrefix('!', ref argPos) || message.HasMentionPrefix(client.CurrentUser, ref argPos))) return;

            var context = new CommandContext(client, message);
            var result = await command.ExecuteAsync(context, argPos, services);
            if (!result.IsSuccess)
            {
                Console.WriteLine(DateTime.Now + " Error:" + result.ErrorReason);
                await context.Channel.SendMessageAsync("We have some problems, please contact with admins.");
            }
        }

    }
}
