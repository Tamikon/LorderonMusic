using DatabaseService;
using DatabaseService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApplication1.Areas.Authorization.Models
{
    public class UserStore
    {
        private readonly IUserRepository _userRepository;
        private readonly IPlaylistRepository _playlistRepository;
        private readonly IMusicRepository _trackRepository;

        public UserStore(IUserRepository userRepository, IPlaylistRepository playlistRepository, IMusicRepository trackRepository)
        {
            _userRepository = userRepository;
            _playlistRepository = playlistRepository;
            _trackRepository = trackRepository;
        }

        public async Task AddOrUpdateUser(User user)
        {
            var existingUser = await _userRepository.GetUser(user.DiscordId);
            if (existingUser == null)
            {
                await _userRepository.AddUser(user);
            }
            else
            {
                existingUser.Username = user.Username;
                existingUser.AvatarUrl = string.IsNullOrEmpty(user.AvatarUrl) ? "https://cdn.discordapp.com/embed/avatars/0.png" : user.AvatarUrl;
                existingUser.FirstAuthorizationDate = user.FirstAuthorizationDate;

                foreach (var server in user.Servers)
                {
                    var existingServer = existingUser.Servers.Find(s => s.DiscordServerId == server.DiscordServerId);
                    if (existingServer == null)
                    {
                        existingUser.Servers.Add(server);
                    }
                    else
                    {
                        existingServer.Name = server.Name;
                        existingServer.AvatarUrl = server.AvatarUrl;
                        foreach (var playlist in server.Playlists)
                        {
                            var existingPlaylist = existingServer.Playlists.Find(p => p.Id == playlist.Id);
                            if (existingPlaylist == null)
                            {
                                existingServer.Playlists.Add(playlist);
                            }
                            else
                            {
                                existingPlaylist.Name = playlist.Name;
                                existingPlaylist.Musics = playlist.Musics;
                            }
                        }
                    }
                }

                await _userRepository.UpdateUser(existingUser);
            }
        }

        public async Task AddPlaylist(Playlist playlist)
        {
            await _playlistRepository.AddPlaylist(playlist);
        }

        public async Task AddTrack(Music music)
        {
            await _trackRepository.AddTrack(music);
        }

        public async Task<List<Playlist>> GetUserPlaylists(string discordId)
        {
            var user = await _userRepository.GetUser(discordId);
            if (user == null)
                return new List<Playlist>();

            var playlists = new List<Playlist>();
            foreach (var server in user.Servers)
            {
                playlists.AddRange(await _playlistRepository.GetAllPlaylists());
            }

            return playlists;
        }

        public async Task<List<Music>> GetPlaylistTracks(int playlistId)
        {
            var playlist = await _playlistRepository.GetPlaylist(playlistId);
            return playlist?.Musics ?? new List<Music>();
        }
    }
}
