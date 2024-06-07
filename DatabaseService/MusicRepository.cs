using DatabaseService.Data;
using DatabaseService.Models;
using Microsoft.EntityFrameworkCore;

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

        public async Task DeleteTrack(int id)
        {
            var track = await _context.Musics.FindAsync(id);
            if (track != null)
            {
                _context.Musics.Remove(track);
                await _context.SaveChangesAsync();
            }
        }
    }
}
