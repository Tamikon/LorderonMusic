using DatabaseService;
using DatabaseService.Models;

namespace WebApplication1.Areas.Authorization.Models
{
    public class UserStore
    {
        private readonly IUserRepository _userRepository;

        public UserStore(IUserRepository userRepository)
        {
            _userRepository = userRepository;
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
    }
}
