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
            return await _context.Users
                .Include(u => u.Servers)
                .ThenInclude(s => s.Playlists)
                .FirstOrDefaultAsync(u => u.DiscordId == discordId);
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await _context.Users
                .Include(u => u.Servers)
                .ThenInclude(s => s.Playlists)
                .ToListAsync();
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

        public async Task<Playlist> GetPlaylist(int playlistId)
        {
            return await _context.Playlists
                .Include(p => p.Musics)
                .FirstOrDefaultAsync(p => p.Id == playlistId);
        }

        public async Task AddPlaylist(Playlist playlist)
        {
            _context.Playlists.Add(playlist);
            await _context.SaveChangesAsync();
        }

        public async Task AddMusic(Music music)
        {
            _context.Musics.Add(music);
            await _context.SaveChangesAsync();
        }
        public async Task<Server> GetServerById(int serverId)
        {
            return await _context.Servers.Include(s => s.Playlists).FirstOrDefaultAsync(s => s.Id == serverId);
        }
        public async Task<List<Server>> GetAllServers()
        {
            return await _context.Servers
                .Include(s => s.Playlists)
                .ToListAsync();
        }
    }
}
