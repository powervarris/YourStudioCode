using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YourStudioFinal.Controllers.Home;
using YourStudioFinal.data;
using YourStudioFinal.Models;
using YourStudioFinal.Models.Gallery;

namespace YourStudioFinal.Controllers.Gallery;

public class GalleryCustomerController : Controller
{
    private readonly ILogger<GalleryCustomerController> _logger;
    
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly ApplicationDbContext _context;
    public GalleryCustomerController(ILogger<GalleryCustomerController> logger , UserManager<User> userManager, SignInManager<User> signInManager, ApplicationDbContext context)
    {
        _logger = logger;
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
    }
    
    public async Task<IActionResult> Index()
    {
        var gallery = await _context.Gallery.FirstOrDefaultAsync();
        
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

        if (gallery == null)
        {
            gallery = new Models.Gallery.Gallery();
            gallery.Id = Guid.NewGuid().ToString();
            gallery.GalleryFiles = new();
            _context.Gallery.Add(gallery);
            await _context.SaveChangesAsync();
            gallery = await _context.Gallery.FirstOrDefaultAsync();
        }
        else
        {
            var files = _context.GalleryFiles.Where(e => e.GalleryId == gallery.Id);
            gallery.GalleryFiles = files.ToList();
        }

        return View(gallery);
    }
    
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Account");
    }

    public async Task<ActionResult> SoloIndex()
    {
        var gallery = await _context.Gallery.FirstOrDefaultAsync();
        
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

        if (gallery == null)
        {
            gallery = new Models.Gallery.Gallery();
            gallery.Id = Guid.NewGuid().ToString();
            gallery.GalleryFiles = new();
            _context.Gallery.Add(gallery);
            await _context.SaveChangesAsync();
            gallery = await _context.Gallery.FirstOrDefaultAsync();
        }
        else
        {
            var files = _context.GalleryFiles.Where(e => e.GalleryId == gallery.Id).Where(e => e.category == "Solo");
            gallery.GalleryFiles = files.ToList();
        }

        return View(gallery);
    }
    
    public async Task<ActionResult> FurIndex()
    {
        var gallery = await _context.Gallery.FirstOrDefaultAsync();
        
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

        if (gallery == null)
        {
            gallery = new Models.Gallery.Gallery();
            gallery.Id = Guid.NewGuid().ToString();
            gallery.GalleryFiles = new();
            _context.Gallery.Add(gallery);
            await _context.SaveChangesAsync();
            gallery = await _context.Gallery.FirstOrDefaultAsync();
        }
        else
        {
            var files = _context.GalleryFiles.Where(e => e.category == "Fur Families").Where(e => e.GalleryId == gallery.Id);
            gallery.GalleryFiles = files.ToList();
        }

        return View(gallery);
    }
    
    public async Task<ActionResult> FamilyIndex()
    {
        var gallery = await _context.Gallery.FirstOrDefaultAsync();
        
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

        if (gallery == null)
        {
            gallery = new Models.Gallery.Gallery();
            gallery.Id = Guid.NewGuid().ToString();
            gallery.GalleryFiles = new();
            _context.Gallery.Add(gallery);
            await _context.SaveChangesAsync();
            gallery = await _context.Gallery.FirstOrDefaultAsync();
        }
        else
        {
            var files = _context.GalleryFiles.Where(e => e.category == "Family").Where(e => e.GalleryId == gallery.Id);
            gallery.GalleryFiles = files.ToList();
        }

        return View(gallery);
    }
    
    public async Task<ActionResult> CoupleIndex()
    {
        var gallery = await _context.Gallery.FirstOrDefaultAsync();
        
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

        if (gallery == null)
        {
            gallery = new Models.Gallery.Gallery();
            gallery.Id = Guid.NewGuid().ToString();
            gallery.GalleryFiles = new();
            _context.Gallery.Add(gallery);
            await _context.SaveChangesAsync();
            gallery = await _context.Gallery.FirstOrDefaultAsync();
        }
        else
        {
            var files = _context.GalleryFiles.Where(e => e.category == "Couples").Where(e => e.GalleryId == gallery.Id);
            gallery.GalleryFiles = files.ToList();
        }

        return View(gallery);
    }
    
    public async Task<ActionResult> ChildIndex()
    {
        var gallery = await _context.Gallery.FirstOrDefaultAsync();
        
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

        if (gallery == null)
        {
            gallery = new Models.Gallery.Gallery();
            gallery.Id = Guid.NewGuid().ToString();
            gallery.GalleryFiles = new();
            _context.Gallery.Add(gallery);
            await _context.SaveChangesAsync();
            gallery = await _context.Gallery.FirstOrDefaultAsync();
        }
        else
        {
            var files = _context.GalleryFiles.Where(e => e.category == "Child").Where(e => e.GalleryId == gallery.Id);
            gallery.GalleryFiles = files.ToList();
        }

        return View(gallery);
    }

}