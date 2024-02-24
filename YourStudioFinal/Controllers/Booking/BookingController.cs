using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using YourStudioFinal.Models;

namespace YourStudioFinal.Controllers.Booking;

// [Authorize]
public class BookingController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    public BookingController(UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }
    
    // GET
    public async Task<ViewResult> Index()
    {
        var UserDetails = await _userManager.GetUserAsync(User);
        if (UserDetails != null)
        {
            ViewBag.User = UserDetails;
            ViewBag.isLogged = true;
        }
        return View();
    }
    
    public IActionResult BList()
    {
        return View();
    }
}