using DatabaseService;
using DatabaseService.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApplication1.Areas.Home.Controllers
{
    [Area("Home")]
    public class HomeController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IPlaylistRepository _playlistRepository;
        private readonly IMusicRepository _musicRepository;

        public HomeController(IUserRepository userRepository, IPlaylistRepository playlistRepository, IMusicRepository musicRepository)
        {
            _userRepository = userRepository;
            _playlistRepository = playlistRepository;
            _musicRepository = musicRepository;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var servers = await _userRepository.GetServersByUserId(userId);
            return View(servers);
        }

        public async Task<IActionResult> ServerDetails(int serverId)
        {
            var server = await _userRepository.GetServer(serverId);
            if (server == null)
            {
                return NotFound();
            }

            return View(server);
        }

        [HttpPost]
        public async Task<IActionResult> AddPlaylist(int serverId, string playlistName)
        {
            var server = await _userRepository.GetServer(serverId);
            if (server == null)
            {
                return NotFound();
            }

            var playlist = new Playlist { Name = playlistName, ServerId = serverId };
            await _playlistRepository.AddPlaylist(playlist);

            return RedirectToAction("ServerDetails", new { serverId = serverId });
        }

        [HttpGet]
        public async Task<IActionResult> MusicDetails(int musicId)
        {
            var music = await _musicRepository.GetTrack(musicId);
            if (music == null)
            {
                return NotFound();
            }

            return View(music);
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

