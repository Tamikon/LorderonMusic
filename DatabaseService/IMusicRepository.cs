using DatabaseService.Models;

namespace DatabaseService
{
    public interface IMusicRepository
    {
        Task AddTrack(Music Musics);
        Task<Music> GetTrack(int id);
        Task DeleteTrack(int id);
    }
}