using DatabaseService.Models;

namespace DatabaseService
{
    public interface IUserRepository
    {
        Task AddUser(User user);
        Task<User> GetUser(string discordId);
        Task UpdateUser(User user);
        Task<Server> GetServer(int serverId);
        Task<List<Server>> GetServersByUserId(string userId);
    }
}
