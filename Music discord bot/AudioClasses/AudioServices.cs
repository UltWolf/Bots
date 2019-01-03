using Discord;
using Discord.Audio;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Music_discord_bot.AudioClasses
{
    public class AudioServices
    {
        private readonly ConcurrentDictionary<ulong, IAudioClient> ConnectedChannels = new ConcurrentDictionary<ulong, IAudioClient>();
        private List<string> Music = new List<string>();
        private IVoiceChannel currentChannel;
        private string currentSong = "";

        public async Task JoinAudio(IGuild guild, IVoiceChannel target)
        {
            IAudioClient client;
            if (ConnectedChannels.TryGetValue(guild.Id, out client))
            {
                return;
            }
            if (target.Guild.Id != guild.Id)
            {
                return;
            }

            var audioClient = await target.ConnectAsync();
            currentChannel = target;
            if (ConnectedChannels.TryAdd(guild.Id, audioClient))
            {
            }
        }

        public async Task StopAudio(IGuild guild)
        {
            
                await LeaveAudio(guild);
                await JoinAudio(guild,currentChannel); 
            }

        public async Task LeaveAudio(IGuild guild)
        {
            IAudioClient client;
            if (ConnectedChannels.TryRemove(guild.Id, out client))
            {
                await client.StopAsync();
                //await Log(LogSeverity.Info, $"Disconnected from voice on {guild.Name}.");
            }
        }
        public async Task AddToPlayList(IMessageChannel channel,string path)
        {
            if (!File.Exists(path))
            {
                await channel.SendMessageAsync("File does not exist.");
                return;
            }
            this.Music.Add(path);
        }

        public async Task SendAudioAsync(IGuild guild, IMessageChannel channel, int id)
        { 
            
            IAudioClient client;
            if (ConnectedChannels.TryGetValue(guild.Id, out client))
            { 
                using (var ffmpeg = CreateProcess(Music[id]))
                using (var stream = client.CreatePCMStream(AudioApplication.Music))
                {
                    this.currentSong = Music[id];
                    try { await ffmpeg.StandardOutput.BaseStream.CopyToAsync(stream); }
                    finally { await stream.FlushAsync();
                        await Next(guild, channel, id++);
                    }
                }
              
            }
        } 
        private async Task Next(IGuild guild, IMessageChannel channel, int id)
        {
           await SendAudioAsync(guild,  channel, id);
        }
        private Process CreateProcess(string path)
        {
            return Process.Start(new ProcessStartInfo
            {
                FileName = "ffmpeg.exe",
                Arguments = $"-hide_banner -loglevel panic -i \"{path}\" -ac 2 -f s16le -ar 48000 pipe:1",
                UseShellExecute = false,
                RedirectStandardOutput = true
            });
        } 
    }
}
