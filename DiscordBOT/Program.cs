using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace DiscordBOT
{
    class Program
    {
        public static void Main(string[] args)
               => new Program().MainAsync().GetAwaiter().GetResult();
        private CommandService command;
        public async Task MainAsync()
        {
            command = new CommandService(new CommandServiceConfig()
            { LogLevel = LogSeverity.Debug,
                DefaultRunMode = RunMode.Async,
                CaseSensitiveCommands = true
            });
            var client = new DiscordSocketClient(); 
            client.Log += Log;
            client.MessageReceived += MessageReceived;
            client.Ready += () =>
            {
                Console.WriteLine("Bot is connected!");
                return Task.CompletedTask;
            };
            string token =""; 
            using (FileStream fs = new FileStream(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location).Replace(@"bin\Debug\netcoreapp2.1", @"Data\Token.txt"), FileMode.Open,FileAccess.Read))
            {
                using(StreamReader sr = new StreamReader(fs))
                {
                    token = sr.ReadToEnd();
                }
             
            }
            

            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();
             
            await Task.Delay(-1);
        }
        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;

        }
        private async Task MessageReceived(SocketMessage message)
        {
            if (message.Content == "!ping")
            {
                await message.Channel.SendMessageAsync("Pong!");
            }
        }
    }
}
