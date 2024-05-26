using System.ComponentModel.DataAnnotations;

namespace DatabaseService.Models
{
    public class Music
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string YouTubeLink { get; set; }
        public int PlaylistId { get; set; }
        public Playlist Playlist { get; set; }
    }
}
