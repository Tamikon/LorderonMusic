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
        private readonly ITrackRepository _trackRepository;

        public UserStore(IUserRepository userRepository, IPlaylistRepository playlistRepository, ITrackRepository trackRepository)
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
                existingUser.Servers = user.Servers;

                await _userRepository.UpdateUser(existingUser);
            }
        }

        public async Task AddPlaylist(Playlist playlist)
        {
            await _playlistRepository.AddPlaylist(playlist);
        }

        public async Task AddTrack(Track track)
        {
            await _trackRepository.AddTrack(track);
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

        public async Task<List<Track>> GetPlaylistTracks(int playlistId)
        {
            var playlist = await _playlistRepository.GetPlaylist(playlistId);
            return playlist?.Tracks ?? new List<Track>();
        }
    }
}
