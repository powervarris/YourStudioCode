using Microsoft.AspNetCore.Mvc;

namespace YourStudioFinal.Controllers.Packages;

public class PackagesController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}