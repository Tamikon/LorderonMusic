using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Areas.PlayList.Controllers;
public class PlayListController : Controller
{
    // GET: PlayListController
    [Area("PlayList")]
    public ActionResult Index()
    {
        return View();
    }

}
