using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace WebApplication1.Areas.Home.Controllers
{
    [Area("Home")]
    public class HomeController : Controller
    {

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
