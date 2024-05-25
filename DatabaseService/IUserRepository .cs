using DatabaseService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatabaseService
{
    public interface IUserRepository
    {
        Task AddUser(User user);
        Task<User> GetUser(string discordId);
        Task<List<User>> GetAllUsers();
        Task UpdateUser(User user); // Новый метод для обновления пользователя
    }
}
