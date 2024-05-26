using Microsoft.AspNetCore.Mvc;
using DatabaseService;
using DatabaseService.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace WebApplication1.Areas.Home.Controllers
{
    [Area("Home")]
    public class HomeController : Controller
    {
        private readonly IUserRepository _userRepository;

        public HomeController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // Метод для отображения всех серверов
        public async Task<IActionResult> Index()
        {
            var servers = await _userRepository.GetAllServers();
            return View(servers);
        }

        // Метод для отображения плейлистов конкретного сервера
        public async Task<IActionResult> ServerDetails(int serverId)
        {
            var server = await _userRepository.GetServer(serverId);
            if (server == null)
            {
                return NotFound();
            }

            return View(server);
        }

        // Метод для отображения треков в плейлисте
        public async Task<IActionResult> PlaylistDetails(int playlistId)
        {
            var playlist = await _userRepository.GetPlaylist(playlistId);
            if (playlist == null)
            {
                return NotFound();
            }

            return View(playlist);
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
            await _userRepository.AddPlaylist(playlist);

            return RedirectToAction("ServerDetails", new { serverId = serverId });
        }
        [HttpPost]
        public async Task<IActionResult> AddMusic(int playlistId, string youtubeLink, string title)
        {
            if (string.IsNullOrEmpty(youtubeLink) || string.IsNullOrEmpty(title))
            {
                ModelState.AddModelError(string.Empty, "Title and YouTube link cannot be empty.");
                return RedirectToAction("PlaylistDetails", new { playlistId = playlistId });
            }

            var playlist = await _userRepository.GetPlaylist(playlistId);
            if (playlist == null)
            {
                return NotFound();
            }

            var music = new Music { Title = title, PlaylistId = playlistId, YouTubeLink = youtubeLink };
            await _userRepository.AddMusic(music);

            return RedirectToAction("PlaylistDetails", new { playlistId = playlistId });
        }
    }
}
