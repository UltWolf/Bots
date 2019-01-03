using Discord;
using Discord.Audio;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Music_discord_bot.Commands
{
    public class MusicModule:ModuleBase
    {
        [Command("Log in ")]
        public async Task logInChannel(IVoiceChannel channel = null)
        {
             
            var voiceChannel  = await  Context.Client.GetChannelAsync(529744496188063744);
            if (channel == null) { await Context.Channel.SendMessageAsync("User must be in a voice channel, or a voice channel must be passed as an argument."); return; }
 
            var audioClient = await channel.ConnectAsync();
            await SendAsync(audioClient, "music/1.mp3");

        }

        [Command("Play")]
        private async Task SendAsync(IAudioClient client, string path)
        {
            var ffmpeg = CreateStream(path);
            var output = ffmpeg.StandardOutput.BaseStream;
            var discord = client.CreatePCMStream(AudioApplication.Mixed);
            await output.CopyToAsync(discord);
            await discord.FlushAsync();
        }
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
