using DatabaseService;
using Microsoft.AspNetCore.Mvc;
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
    }
}

