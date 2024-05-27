using DatabaseService.Data;
using DatabaseService.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatabaseService
{
    public class MusicRepository : IMusicRepository
    {
        private readonly ApplicationDbContext _context;

        public MusicRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddTrack(Music track)
        {
            _context.Musics.Add(track);
            await _context.SaveChangesAsync();
        }

        public async Task<Music> GetTrack(int id)
        {
            return await _context.Musics.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<List<Music>> GetAllTracks()
        {
            return await _context.Musics.ToListAsync();
        }

        public async Task<List<Music>> GetTracksByPlaylist(int playlistId)
        {
            return await _context.Musics.Where(m => m.PlaylistId == playlistId).ToListAsync();
        }

        public async Task UpdateTrack(Music track)
        {
            _context.Musics.Update(track);
            await _context.SaveChangesAsync();
        }
    }
}
