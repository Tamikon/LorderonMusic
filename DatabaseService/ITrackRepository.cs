using DatabaseService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatabaseService
{
    public interface ITrackRepository
    {
        Task AddTrack(Track track);
        Task<Track> GetTrack(int id);
        Task<List<Track>> GetAllTracks();
        Task UpdateTrack(Track track);
    }
}