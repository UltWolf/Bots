using Discord;
using Discord.Audio;
using Discord.Commands;
using Music_discord_bot.AudioClasses;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Music_discord_bot.Commands
{
    [Group("Music")]
    public class MusicModule:ModuleBase
    {
        private readonly AudioServices _service;

       
        public MusicModule(AudioServices service)
        {
            _service = service;
        }
         
        [Command("join", RunMode = RunMode.Async)]
        public async Task JoinCmd()
        {
            await _service.JoinAudio(Context.Guild, (Context.User as IVoiceState).VoiceChannel);
        }
         
        [Command("leave", RunMode = RunMode.Async)]
        public async Task LeaveCmd()
        {
            await _service.LeaveAudio(Context.Guild);
        }

        [Command("play", RunMode = RunMode.Async)]
        public async Task PlayCmd()
        {
            await _service.SendAudioAsync(Context.Guild, Context.Channel, 0);
        } 

        [Command("add")]
        public async Task PlayCmd([Remainder]string name)
        {
           await  _service.AddToPlayList(Context.Channel,name.ToString());
        }

        [Command("stop")]
        public async Task StopPlay()
        {
            await _service.StopAudio(Context.Guild);
        }
        [Command("login")]
        public async Task logInChannel(IVoiceChannel channel = null)
        {
             
            var voiceChannel  = await  Context.Client.GetChannelAsync(529744496188063744);
            if (channel == null) { await Context.Channel.SendMessageAsync("User must be in a voice channel, or a voice channel must be passed as an argument."); return; }
 
            var audioClient = await channel.ConnectAsync(); 

        }

        //[Command("Play")]
        //private async Task SendAsync( string path)
        //{
        //    if (!File.Exists(path))
        //    {
        //        await channel.SendMessageAsync("File does not exist.");
        //        return;
        //    }
        //    IAudioClient client;
        //    if (ConnectedChannels.TryGetValue(guild.Id, out client))
        //    {

        //        var ffmpeg = CreateStream(path);
        //        using (var stream = client.CreatePCMStream(AudioApplication.Music))
        //        {
        //            try { await ffmpeg.StandardOutput.BaseStream.CopyToAsync(stream); }
        //            finally { await stream.FlushAsync(); }
        //        }
        //    }
        //    var output = ffmpeg.StandardOutput.BaseStream;
        //    var discord = client.CreatePCMStream(AudioApplication.Mixed);
        //    await output.CopyToAsync(discord);
        //    await discord.FlushAsync();
        //}
        private Process CreateStream(string path)
        {
            var ffmpeg = new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = $"-i {path} -ac 2 -f s16le -ar 48000 pipe:1",
                UseShellExecute = false,
                RedirectStandardOutput = true,
            };
            return Process.Start(ffmpeg);
        }
    }
}
