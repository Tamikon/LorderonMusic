using DatabaseService;
using DatabaseService.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WebApplication1.Areas.Home.Models;

namespace WebApplication1.Areas.Home.Controllers
{
    [Area("Home")]
    public class AddMusicController : Controller
    {
        private readonly IMusicRepository _musicRepository;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _youtubeApiKey = "AIzaSyA2TxFlqiE5wW2zfQkjC-_LmKkXfDHPxMk";

        public AddMusicController(IMusicRepository musicRepository, IHttpClientFactory httpClientFactory)
        {
            _musicRepository = musicRepository;
            _httpClientFactory = httpClientFactory;
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
            return RedirectToAction("PlaylistDetails", "Home", new { area = "Home", playlistId });
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
