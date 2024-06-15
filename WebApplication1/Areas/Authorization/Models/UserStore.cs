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
                user.FirstAuthorizationDate = DateTime.UtcNow;
                await _userRepository.AddUser(user);
            }
            else
            {
                existingUser.Username = user.Username;
                existingUser.AvatarUrl = string.IsNullOrEmpty(user.AvatarUrl) ? "https://cdn.discordapp.com/embed/avatars/0.png" : user.AvatarUrl;

                foreach (var server in user.Servers)
                {
                    var existingServer = existingUser.Servers.Find(s => s.DiscordServerId == server.DiscordServerId);
                    if (existingServer == null)
                    {
                        existingUser.Servers.Add(server);
                    }
                }

                await _userRepository.UpdateUser(existingUser);
            }
        }
    }
}
