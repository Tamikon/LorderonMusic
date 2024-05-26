using System.Collections.Generic;
using System.Threading.Tasks;
using DatabaseService.Models;

namespace DatabaseService
{
    public interface IUserRepository
    {
        Task AddUser(User user);
        Task<User> GetUser(string discordId);
        Task<List<User>> GetAllUsers();
        Task UpdateUser(User user);
        Task<Server> GetServer(int serverId);
        Task<Playlist> GetPlaylist(int playlistId);
        Task AddPlaylist(Playlist playlist);
        Task AddMusic(Music music);
        Task<Server> GetServerById(int serverId);
        Task<List<Server>> GetAllServers();
    }
}
