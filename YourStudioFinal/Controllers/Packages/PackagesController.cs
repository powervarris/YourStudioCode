using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using YourStudioFinal.data;
using YourStudioFinal.Models;

namespace YourStudioFinal.Controllers.Packages;

public class PackagesController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly ApplicationDbContext _context;
    public PackagesController(UserManager<User> userManager, SignInManager<User> signInManager, ApplicationDbContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
    }
    public async Task<IActionResult> Index()
    {
        var UserDetails = await _userManager.GetUserAsync(User);
        if (UserDetails != null)
        {
            ViewBag.User = UserDetails;
            ViewBag.isLogged = true;
        }
        else
        {
            ViewBag.isLogged = false;
        }
        return View();
    }
    
    public async Task<IActionResult> goToBooking()
    {
        return RedirectToAction("Index", "Booking");
    }
}