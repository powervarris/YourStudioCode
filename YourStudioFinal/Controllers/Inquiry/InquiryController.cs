using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

    [Authorize]
    public IActionResult clientInquiry()
    {
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
    
    public IActionResult Inquirylist()
    {
        return View();
    }
    
    }