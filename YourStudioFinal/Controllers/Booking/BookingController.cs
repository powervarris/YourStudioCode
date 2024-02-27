using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
    
    [HttpPost]
    public async Task<IActionResult> AddBooking(BookingModel bookingModel, [FromForm(Name = "image")]IFormFile image)
    {
        bookingModel.dateCreated = DateTime.Now;
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
    
    public IActionResult BList()
    {
        return View();
    }
}