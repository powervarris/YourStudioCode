using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YourStudioFinal.data;
using YourStudioFinal.Models;
using YourStudioFinal.Models.Booking;

namespace YourStudioFinal.Controllers.Booking;

// [Authorize]
public class BookingController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly ApplicationDbContext _context;
    public BookingController(UserManager<User> userManager, SignInManager<User> signInManager, ApplicationDbContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
    }
    
    // GET
    public async Task<ActionResult> Index()
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
    public async Task<IActionResult> AddBooking(BookingModel bookingModel, [FromForm(Name = "image")]IFormFile image)
    {
        bookingModel.dateCreated = DateTime.Now;
        bookingModel.status = "Pending";
        bookingModel.accountUser = _userManager.GetUserAsync(HttpContext.User).Result;
        new PaymentModel();
        var Payment = new PaymentModel();
        if (image != null)
        {
            var fileName = Path.GetFileName(image.FileName);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", fileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
            Payment.fileName = fileName;
            Payment.Booking = bookingModel;
            bookingModel.payment = Payment;
            bookingModel.paymentID = Payment.Id;
            _context.Add(Payment);
        }
        _context.Add(bookingModel);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }
    
    public async Task<ActionResult> BList()
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
        return View(_context.Booking.Include(x => x.accountUser)
            .Include(x => x.payment)
            .ToList());
    }

    [HttpPost]
    public async Task<IActionResult> acceptBooking(BookingModel bookingModel)
    {
        // BookingModel ? bookingChanges = _context.Booking.FirstOrDefault(u => u.Id == bookingModel.Id);
        // if (bookingChanges != null)
        // {
            bookingModel.status = "Accepted";
            _context.Update(bookingModel);
            _context.SaveChanges();
        // }
        return RedirectToAction("BList");
    }

    public async Task<ActionResult> Booking()
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
        return View(_context.Booking.Include(x => x.accountUser)
            .Include(x => x.payment)
            .ToList());
    }
}