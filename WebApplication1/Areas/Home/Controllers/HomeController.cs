using DatabaseService.Models;
using DatabaseService;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

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

        [HttpPost]
        public async Task<IActionResult> SearchServers(string searchQuery)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var servers = await _userRepository.GetServersByUserId(userId);
            if (!string.IsNullOrEmpty(searchQuery))
            {
                servers = servers.Where(s => s.Name.Contains(searchQuery, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            return View("Index", servers);
        }
    }
}
