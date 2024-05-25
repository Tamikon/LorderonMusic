using DatabaseService.Models;

namespace WebApplication1.Areas.Home.Models
{
    public class User
    {
        public string DiscordId { get; set; }
        public string Username { get; set; }
        public string AvatarUrl { get; set; }
        public DateTime FirstAuthorizationDate { get; set; }
        public List<Server> Servers { get; set; } = new List<Server>();
    }
}
