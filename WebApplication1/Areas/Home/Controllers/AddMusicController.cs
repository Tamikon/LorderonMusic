using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Areas.Home.Controllers
{
    public class AddMusicController : Controller
    {
        // GET: AddMusic
        public ActionResult Index()
        {
            return View();
        }

        // GET: AddMusic/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AddMusic/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AddMusic/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AddMusic/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AddMusic/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AddMusic/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AddMusic/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
