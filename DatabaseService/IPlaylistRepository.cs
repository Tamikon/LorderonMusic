using DatabaseService.Models;

namespace DatabaseService
{
    public interface IPlaylistRepository
    {
        Task AddPlaylist(Playlist playlist);
        Task<Playlist> GetPlaylist(int id);
        Task DeletePlaylist(int id);

        Task UpdatePlaylist(Playlist playlist);
    }
}
