using DatabaseService;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Areas.Home.Controllers
{
    [Area("Home")]
    public class MusicDetailsController : Controller
    {
        private readonly IMusicRepository _musicRepository;

        public MusicDetailsController(IMusicRepository musicRepository)
        {
            _musicRepository = musicRepository;
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
    }
}
