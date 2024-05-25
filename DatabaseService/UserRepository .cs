using DatabaseService.Data;
using DatabaseService.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatabaseService
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddUser(User user)
        {
            if (!await _context.Users.AnyAsync(u => u.DiscordId == user.DiscordId))
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<User> GetUser(string discordId)
        {
            return await _context.Users.Include(u => u.Servers).FirstOrDefaultAsync(u => u.DiscordId == discordId);
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await _context.Users.Include(u => u.Servers).ToListAsync();
        }

        public async Task UpdateUser(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
