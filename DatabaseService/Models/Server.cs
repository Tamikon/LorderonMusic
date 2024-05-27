using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DatabaseService.Models
{
    public class Server
    {
        [Key]
        public int Id { get; set; }
        public string DiscordServerId { get; set; }
        public string Name { get; set; }
        public string AvatarUrl { get; set; }
        public List<Playlist> Playlists { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
