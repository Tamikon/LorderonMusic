namespace WebApplication1.Areas.Home.Models
{
    public class Server
    {
        public string DiscordServerId { get; set; }
        public string Name { get; set; }
        public string AvatarUrl { get; set; } // Добавлено новое поле
        public List<Playlist> Playlists { get; set; } = new List<Playlist>();
    }
}
