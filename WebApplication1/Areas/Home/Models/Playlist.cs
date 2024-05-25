using DatabaseService.Models;

namespace WebApplication1.Areas.Home.Models
{
    public class Playlist
    {
        public string Name { get; set; }
        public List<Track> Tracks { get; set; } // Добавлено: треки в плейлисте
    }
}
