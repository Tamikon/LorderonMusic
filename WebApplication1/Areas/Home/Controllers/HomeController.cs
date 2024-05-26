using DatabaseService;
using DatabaseService.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VideoLibrary;

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
            var servers = await _userRepository.GetAllServers();
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
        public IActionResult AddMusicForm(int playlistId)
        {
            var model = new Music { PlaylistId = playlistId };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddMusic(int playlistId, string youTubeLink)
        {
            if (string.IsNullOrEmpty(youTubeLink))
            {
                ModelState.AddModelError(string.Empty, "YouTube link cannot be empty.");
                return RedirectToAction("AddMusicForm", new { area = "Home", playlistId });
            }

            var youtube = YouTube.Default;
            var video = await Task.Run(() => youtube.GetVideo(youTubeLink));
            var title = video.Title;
            //var author = video.Autho;

            var music = new Music
            {
                PlaylistId = playlistId,
                Title = title,
                //Artist = author,
                YouTubeLink = youTubeLink
            };

            await _musicRepository.AddTrack(music);
            return RedirectToAction("PlaylistDetails", new { area = "Home", playlistId });
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
