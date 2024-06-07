using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Areas.Home.Controllers
{
    [Area("Home")]
    public class AboutProjectController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
