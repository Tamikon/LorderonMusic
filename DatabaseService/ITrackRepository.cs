using DatabaseService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatabaseService
{
    public interface ITrackRepository
    {
        Task AddTrack(Music Musics);
        Task<Music> GetTrack(int id);
        Task<List<Music>> GetAllTracks();
        Task UpdateTrack(Music Musics);
    }
}