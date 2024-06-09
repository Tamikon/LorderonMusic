using System.Text.Json;
using System.Text.RegularExpressions;
using WebApplication1.Areas.Home.Models;

namespace WebApplication1
{
    public class YouTubeService 
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _youtubeApiKey;

        public YouTubeService(IHttpClientFactory httpClientFactory, string youtubeApiKey)
        {
            _httpClientFactory = httpClientFactory;
            _youtubeApiKey = youtubeApiKey;
        }

        public string GetYouTubeVideoId(string url)
        {
            var uri = new Uri(url);
            var query = System.Web.HttpUtility.ParseQueryString(uri.Query);
            return query["v"];
        }

        public async Task<YouTubeVideoInfo> GetYouTubeVideoInfo(string videoId)
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

        public bool IsValidYouTubeLink(string url)
        {
            var regex = new Regex(@"^(https?:\/\/)?(www\.)?(youtube\.com\/watch\?v=|youtu\.be\/|music\.youtube\.com\/watch\?v=).+$");
            if (!regex.IsMatch(url))
            {
                return false;
            }

            var videoId = GetYouTubeVideoId(url);
            return !string.IsNullOrEmpty(videoId);
        }
    }
}
