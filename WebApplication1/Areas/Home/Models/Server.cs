using DatabaseService.Models;

namespace WebApplication1.Areas.Home.Models
{
    public class Server
    {
        public string DiscordServerId { get; set; }
        public string Name { get; set; }
        public List<Playlist> Playlists { get; set; } = new List<Playlist>();
    }
}
