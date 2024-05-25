using DatabaseService.Data;
using DatabaseService.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatabaseService
{
    public class TrackRepository : ITrackRepository
    {
        private readonly ApplicationDbContext _context;

        public TrackRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddTrack(Track track)
        {
            _context.Tracks.Add(track);
            await _context.SaveChangesAsync();
        }

        public async Task<Track> GetTrack(int id)
        {
            return await _context.Tracks.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<List<Track>> GetAllTracks()
        {
            return await _context.Tracks.ToListAsync();
        }

        public async Task UpdateTrack(Track track)
        {
            _context.Tracks.Update(track);
            await _context.SaveChangesAsync();
        }
    }
}
