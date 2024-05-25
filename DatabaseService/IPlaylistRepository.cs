using DatabaseService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatabaseService
{
    public interface IPlaylistRepository
    {
        Task AddPlaylist(Playlist playlist);
        Task<Playlist> GetPlaylist(int id);
        Task<List<Playlist>> GetAllPlaylists();
        Task UpdatePlaylist(Playlist playlist);
    }
}
