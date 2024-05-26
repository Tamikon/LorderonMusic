using DatabaseService.Data;
using DatabaseService.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatabaseService
{
    public class PlaylistRepository : IPlaylistRepository
    {
        private readonly ApplicationDbContext _context;

        public PlaylistRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddPlaylist(Playlist playlist)
        {
            _context.Playlists.Add(playlist);
            await _context.SaveChangesAsync();
        }

        public async Task<Playlist> GetPlaylist(int id)
        {
            return await _context.Playlists.Include(p => p.Musics).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Playlist>> GetAllPlaylists()
        {
            return await _context.Playlists.Include(p => p.Musics).ToListAsync();
        }

        public async Task UpdatePlaylist(Playlist playlist)
        {
            _context.Playlists.Update(playlist);
            await _context.SaveChangesAsync();
        }
    }
}
