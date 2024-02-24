using Microsoft.AspNetCore.Mvc;

namespace YourStudioFinal.Controllers
{
    public class GalleryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
