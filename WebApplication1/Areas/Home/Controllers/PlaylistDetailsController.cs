using DatabaseService;
using DatabaseService.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Areas.Home.Controllers
{
    [Area("Home")]
    public class PlaylistDetailsController : Controller
    {
        private readonly IPlaylistRepository _playlistRepository;
        private readonly IMusicRepository _musicRepository;
        private readonly YouTubeService _youTubeService;

        public PlaylistDetailsController(IPlaylistRepository playlistRepository, IMusicRepository musicRepository, YouTubeService youTubeService)
        {
            _playlistRepository = playlistRepository;
            _musicRepository = musicRepository;
            _youTubeService = youTubeService;
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
                musicList = musicList.Where(m => m.Title.Contains(searchQuery, System.StringComparison.OrdinalIgnoreCase) || m.Artist.Contains(searchQuery, System.StringComparison.OrdinalIgnoreCase)).ToList();
            }

            playlist.Musics = musicList;
            return View("PlaylistDetails", playlist);
        }


        [HttpPost]
        public async Task<IActionResult> AddMusic(int playlistId, string youTubeLink)
        {
            if (string.IsNullOrEmpty(youTubeLink) || !_youTubeService.IsValidYouTubeLink(youTubeLink))
            {
                ModelState.AddModelError(string.Empty, "The YouTube link is not valid.");
                return RedirectToAction("PlaylistDetails", new { playlistId });
            }

            var videoId = _youTubeService.GetYouTubeVideoId(youTubeLink);
            var videoInfo = await _youTubeService.GetYouTubeVideoInfo(videoId);

            if (videoInfo == null)
            {
                ModelState.AddModelError(string.Empty, "Unable to retrieve video information.");
                return RedirectToAction("PlaylistDetails", new { playlistId });
            }

            var music = new Music
            {
                PlaylistId = playlistId,
                Title = videoInfo.Title,
                Artist = videoInfo.Author,
                YouTubeLink = youTubeLink
            };

            await _musicRepository.AddTrack(music);
            return RedirectToAction("PlaylistDetails", new { playlistId });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTrack(int trackId, int playlistId)
        {
            await _musicRepository.DeleteTrack(trackId);
            return RedirectToAction("PlaylistDetails", new { playlistId });
        }
    }
}
