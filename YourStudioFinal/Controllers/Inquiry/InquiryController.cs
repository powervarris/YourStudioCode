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
    public ActionResult clientInquiry()
    {
        ViewBag.isLogged = false;
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

    public ActionResult Inquirylist()
    {
        ViewBag.isLogged = false;
        return View();
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

    public async Task<ActionResult> Delete(string id)
    {
        var UserDetails = await _userManager.GetUserAsync(User);
        if (UserDetails == null)
        {
            TempData["Error"] = "You need to login to access this page";
            return RedirectToAction("Index", "Account");
        }

        ViewBag.User = UserDetails;
        ViewBag.isLogged = true;

        var inquiry = _context.Inquiries.Find(id);
        _context.Inquiries.Remove(inquiry);
        _context.SaveChanges();
        return RedirectToAction("iListAdmin");
    }

}
