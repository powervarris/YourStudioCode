using Microsoft.AspNetCore.Mvc;

namespace YourStudioFinal.Controllers.Gallery;

public class GalleryCustomerController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}