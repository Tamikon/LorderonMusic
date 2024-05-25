using System.ComponentModel.DataAnnotations;

namespace DatabaseService.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string DiscordId { get; set; }
        public string Username { get; set; }
        public string AvatarUrl { get; set; }
        public DateTime FirstAuthorizationDate { get; set; }
        public List<string> Guilds { get; set; }
    }
}
