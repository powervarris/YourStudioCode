using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YourStudioFinal.data;
using YourStudioFinal.Models;

namespace YourStudioFinal.Controllers;

// [Authorize]
public class InquiryController : Controller
{
    private readonly ILogger<InquiryController> _logger;
    
    private readonly ApplicationDbContext _context;

    private readonly UserManager<User> _userManager;

    public InquiryController(ILogger<InquiryController> logger, ApplicationDbContext context, UserManager<User> userManager)
    {
        _userManager = userManager;
        _context = context;
        _logger = logger;
    }
    public async Task<ActionResult> clientInquiry()
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
            TempData["Error"] = "You need to login to access this page";
            return RedirectToAction("Index", "Account");
        }
        return View();
    }

    [HttpPost]
    public ActionResult Submit(Inquiry inquiry)
    {
        inquiry.accountUser = _userManager.GetUserAsync(HttpContext.User).Result;
        _context.Add(inquiry);
        _context.SaveChanges();
        return RedirectToAction("clientInquiry");
    }
    
    public async Task<ActionResult> Inquirylist()
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
            TempData["Error"] = "You need to login to access this page";
            return RedirectToAction("Index", "Account");
        }
        return View(_context.Inquiries.Include(e => e.accountUser).ToList());
    }
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> iListAdmin()
    {
        var UserDetails = await _userManager.GetUserAsync(User);
        if (UserDetails == null)
        {
            TempData["Error"] = "You need to login to access this page";
            return RedirectToAction("Index", "Account");
        }

        ViewBag.User = UserDetails;
        ViewBag.isLogged = true;

        var inquiries = _context.Inquiries.Include(e => e.accountUser).ToList();
        return View(inquiries);
    }
    [HttpPost]
    public async Task<IActionResult> DeleteInquiry(string id)
    {
        var inquiry = await _context.Inquiries.FindAsync(id);
        if (inquiry == null)
        {
            return NotFound();
        }

        try
        {
            _context.Inquiries.Remove(inquiry);
            await _context.SaveChangesAsync();

            // Redirect back to the inquiry list page after deletion
            return RedirectToAction("iListAdmin");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error deleting inquiry: {ex.Message}");
        }
    }
}
