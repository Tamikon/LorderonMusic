using DatabaseService.Data;
using DatabaseService.Models;

using Microsoft.EntityFrameworkCore;

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

        public async Task DeletePlaylist(int id)
        {
            var playlist = await _context.Playlists.FindAsync(id);
            if (playlist != null)
            {
                _context.Playlists.Remove(playlist);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdatePlaylist(Playlist playlist)
        {
            _context.Playlists.Update(playlist);
            await _context.SaveChangesAsync();
        }


    }
}
