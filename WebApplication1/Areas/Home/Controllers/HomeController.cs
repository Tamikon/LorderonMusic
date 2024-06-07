using DatabaseService;
using DatabaseService.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using WebApplication1.Areas.Home.Models;

namespace WebApplication1.Areas.Home.Controllers
{
    [Area("Home")]
    public class HomeController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IPlaylistRepository _playlistRepository;
        private readonly IMusicRepository _musicRepository;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _youtubeApiKey = "AIzaSyA2TxFlqiE5wW2zfQkjC-_LmKkXfDHPxMk";

        public HomeController(IUserRepository userRepository, IPlaylistRepository playlistRepository, IMusicRepository musicRepository, IHttpClientFactory httpClientFactory)
        {
            _userRepository = userRepository;
            _playlistRepository = playlistRepository;
            _musicRepository = musicRepository;
            _httpClientFactory = httpClientFactory;
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
        public IActionResult AddMusicForm(int playlistId)
        {
            var model = new Music { PlaylistId = playlistId };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddMusic(int playlistId, string youTubeLink)
        {
            if (string.IsNullOrEmpty(youTubeLink) || !IsValidYouTubeLink(youTubeLink))
            {
                ModelState.AddModelError(string.Empty, "The YouTube link is not valid.");
                return RedirectToAction("AddMusicForm", new { area = "Home", playlistId });
            }

            var videoId = GetYouTubeVideoId(youTubeLink);
            var videoInfo = await GetYouTubeVideoInfo(videoId);

            if (videoInfo == null)
            {
                ModelState.AddModelError(string.Empty, "Unable to retrieve video information.");
                return RedirectToAction("AddMusicForm", new { area = "Home", playlistId });
            }

            var music = new Music
            {
                PlaylistId = playlistId,
                Title = videoInfo.Title,
                Artist = videoInfo.Author,
                YouTubeLink = youTubeLink
            };

            await _musicRepository.AddTrack(music);
            return RedirectToAction("PlaylistDetails", new { area = "Home", playlistId });
        }

        private string GetYouTubeVideoId(string url)
        {
            var uri = new Uri(url);
            var query = System.Web.HttpUtility.ParseQueryString(uri.Query);
            return query["v"];
        }

        private async Task<YouTubeVideoInfo> GetYouTubeVideoInfo(string videoId)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"https://www.googleapis.com/youtube/v3/videos?id={videoId}&key={_youtubeApiKey}&part=snippet");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();
            var jsonDoc = JsonDocument.Parse(content);
            var snippet = jsonDoc.RootElement.GetProperty("items")[0].GetProperty("snippet");

            var title = snippet.GetProperty("title").GetString();
            var author = snippet.GetProperty("channelTitle").GetString();

            return new YouTubeVideoInfo
            {
                Title = title,
                Author = author
            };
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

        private bool IsValidYouTubeLink(string url)
        {
            var regex = new System.Text.RegularExpressions.Regex(@"^(https?:\/\/)?(www\.)?(youtube\.com\/watch\?v=|youtu\.be\/|music\.youtube\.com\/watch\?v=).+$");
            if (!regex.IsMatch(url))
            {
                return false;
            }

            var videoId = GetYouTubeVideoId(url);
            return !string.IsNullOrEmpty(videoId);
        }

    }
}

