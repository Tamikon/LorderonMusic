﻿using DatabaseService.Data;
using DatabaseService.Models;
using Microsoft.EntityFrameworkCore;

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
            return await _context.Users
                .Include(u => u.Servers)
                .ThenInclude(s => s.Playlists)
                .ThenInclude(p => p.Musics)
                .FirstOrDefaultAsync(u => u.DiscordId == discordId);
        }

        public async Task UpdateUser(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<Server> GetServer(int serverId)
        {
            return await _context.Servers
                .Include(s => s.Playlists)
                .FirstOrDefaultAsync(s => s.Id == serverId);
        }
        public async Task<List<Server>> GetServersByUserId(string userId)
        {
            return await _context.Servers
                .Where(s => s.User.DiscordId == userId)
                .Include(s => s.Playlists)
                .ToListAsync();
        }
    }
}
