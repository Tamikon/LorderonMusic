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

        [HttpPost]
        public async Task<IActionResult> SearchMusic(int playlistId, string searchQuery)
        {
            var playlist = await _playlistRepository.GetPlaylist(playlistId);
            if (playlist == null)
            {
                return NotFound();
            }

            var musicList = playlist.Musics;
            if (!string.IsNullOrEmpty(searchQuery))
            {
                musicList = musicList.Where(m => m.Title.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) || m.Artist.Contains(searchQuery, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            playlist.Musics = musicList;
            return View("PlaylistDetails", playlist);
        }
    }
}
