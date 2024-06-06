using System.ComponentModel.DataAnnotations;

namespace DatabaseService.Models
{
    public class Music
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }

        [Required]
        [RegularExpression(@"^(https?:\/\/)?(www\.)?(youtube\.com\/watch\?v=|youtu\.be\/|music\.youtube\.com\/watch\?v=).+$", ErrorMessage = "The YouTube link is not valid.")]
        public string YouTubeLink { get; set; }

        public int PlaylistId { get; set; }
        public Playlist Playlist { get; set; }
    }
}
