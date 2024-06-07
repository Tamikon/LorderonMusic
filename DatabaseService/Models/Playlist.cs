using System.ComponentModel.DataAnnotations;

namespace DatabaseService.Models
{
    public class Playlist
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Music> Musics { get; set; }
        public int ServerId { get; set; }
        public Server Server { get; set; }
    }
}
