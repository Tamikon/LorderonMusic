using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace WebApplication1.Areas.Home.Controllers
{
    public class HomeController : Controller
    {
        [Area("Home")]
        public async Task<IActionResult> IndexAsync()
        {
            return View();
        }

    }
}
