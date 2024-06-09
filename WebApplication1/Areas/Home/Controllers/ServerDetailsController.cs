using DatabaseService.Models;
using DatabaseService;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Areas.Home.Controllers
{
    [Area("Home")]
    public class ServerDetailsController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IPlaylistRepository _playlistRepository;

        public ServerDetailsController(IUserRepository userRepository, IPlaylistRepository playlistRepository)
        {
            _userRepository = userRepository;
            _playlistRepository = playlistRepository;
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

            return RedirectToAction("ServerDetails", new { serverId });
        }

        [HttpPost]
        public async Task<IActionResult> SearchPlaylists(int serverId, string searchQuery)
        {
            var server = await _userRepository.GetServer(serverId);
            if (server == null)
            {
                return NotFound();
            }

            var playlists = server.Playlists;
            if (!string.IsNullOrEmpty(searchQuery))
            {
                playlists = playlists.Where(p => p.Name.Contains(searchQuery, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            server.Playlists = playlists;
            return View("ServerDetails", server);
        }

        [HttpPost]
        public async Task<IActionResult> DeletePlaylist(int playlistId, int serverId)
        {
            await _playlistRepository.DeletePlaylist(playlistId);
            return RedirectToAction("ServerDetails", new { serverId });
        }

        [HttpPost]
        public async Task<IActionResult> EditPlaylist(int playlistId, int serverId, string playlistName)
        {
            var playlist = await _playlistRepository.GetPlaylist(playlistId);
            if (playlist == null)
            {
                return NotFound();
            }

            playlist.Name = playlistName;
            await _playlistRepository.UpdatePlaylist(playlist);

            return RedirectToAction("ServerDetails", new { serverId });
        }

        public async Task<IActionResult> GetPlaylist(int playlistId)
        {
            var playlist = await _playlistRepository.GetPlaylist(playlistId);
            if (playlist == null)
            {
                return NotFound();
            }

            ViewBag.ServerId = playlist.ServerId;
            return PartialView("_EditPlaylistForm", playlist);
        }

        public async Task<IActionResult> GetEditPlaylistForm(int playlistId)
        {
            var playlist = await _playlistRepository.GetPlaylist(playlistId);
            if (playlist == null)
            {
                return NotFound();
            }

            ViewBag.ServerId = playlist.ServerId;
            return PartialView("_EditPlaylistForm", playlist);
        }


    }
}
