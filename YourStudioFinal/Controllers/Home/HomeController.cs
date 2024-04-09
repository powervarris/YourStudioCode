using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using YourStudioFinal.Models;
using YourStudioFinal.Models;

namespace YourStudioFinal.Controllers.Home;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    public HomeController(ILogger<HomeController> logger , UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _logger = logger;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<ViewResult> Index()
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

    public IActionResult Privacy()
    {
        return View();
    }
    
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Account");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    
}