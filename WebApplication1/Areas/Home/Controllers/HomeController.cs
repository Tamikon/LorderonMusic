using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace WebApplication1.Areas.Home.Controllers
{
    public class HomeController : Controller
    {
        [Area("Home")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Playlists()
        {
            return View();
        }

        public IActionResult Reference()
        {
            return View();
        }
    }
}
