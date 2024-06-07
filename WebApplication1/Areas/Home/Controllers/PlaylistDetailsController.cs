using DatabaseService;
using Microsoft.AspNetCore.Mvc;
namespace WebApplication1.Areas.Home.Controllers
{
    [Area("Home")]
    public class PlaylistDetailsController : Controller
    {
        private readonly IPlaylistRepository _playlistRepository;

        public PlaylistDetailsController(IPlaylistRepository playlistRepository)
        {
            _playlistRepository = playlistRepository;
        }

        [HttpGet]
        public async Task<IActionResult> PlaylistDetails(int playlistId)
        {
            var playlist = await _playlistRepository.GetPlaylist(playlistId);
            if (playlist == null)
            {
                return NotFound();
            }

            return View(playlist);
        }
    }
}
