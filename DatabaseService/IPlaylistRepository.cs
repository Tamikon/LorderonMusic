using DatabaseService.Models;

namespace DatabaseService
{
    public interface IPlaylistRepository
    {
        Task AddPlaylist(Playlist playlist);
        Task<Playlist> GetPlaylist(int id);
    }
}
