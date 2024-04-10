using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SendGrid;
using SendGrid.Helpers.Mail;
using YourStudioFinal.data;
using YourStudioFinal.Models;
using YourStudioFinal.Models.Booking;

namespace YourStudioFinal.Controllers.Booking;

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
    
    [Authorize(Roles = "User, Admin")] 
    public async Task<IActionResult> Payment()
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
        return View(_context.Booking.Where(x => x.accountUser.Id == UserDetails.Id).Where(x => x.status == "Accepted").Include(x => x.accountUser).Include(x => x.payment).ToList());
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
    
    // public async Task<ActionResult> Index(string package)
    // {
    //     var UserDetails = await _userManager.GetUserAsync(User);
    //     if (UserDetails != null)
    //     {
    //         ViewBag.User = UserDetails;
    //         ViewBag.isLogged = true;
    //     }
    //     else
    //     {
    //         ViewBag.isLogged = false;
    //         TempData["Error"] = "You need to login to access this page";
    //         return RedirectToAction("Index", "Account");
    //     }
    //     ViewBag.selectedPackage = package;
    //     return View();
    // }
    
    [Authorize(Roles = "User, Admin")]
    [HttpPost]
    public async Task<IActionResult> AddBooking(BookingModel bookingModel, [FromForm(Name = "image")]IFormFile image)
    {
        bookingModel.dateCreated = DateTime.Now;
        bookingModel.status = "Pending";
        bookingModel.accountUser = _userManager.GetUserAsync(HttpContext.User).Result;
        // new PaymentModel();
        // var Payment = new PaymentModel();
        // if (image != null)
        // {
        //     var fileName = Path.GetFileName(image.FileName);
        //     var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", fileName);
        //     using (var fileStream = new FileStream(filePath, FileMode.Create))
        //     {
        //         await image.CopyToAsync(fileStream);
        //     }
        //     Payment.fileName = fileName;
        //     Payment.Booking = bookingModel;
        //     bookingModel.payment = Payment;
        //     bookingModel.paymentID = Payment.Id;
        //     _context.Add(Payment);
        // }
        _context.Add(bookingModel);
        _context.SaveChanges();
        
        var UserDetails = await _userManager.GetUserAsync(User);
        
        //customer side email
        var apiKey = "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";
        var client = new SendGridClient(apiKey);
        var from_email = new EmailAddress("yourstudio.bacoor@gmail.com", "YourStudio");
        var subject = "Booking Confirmation";
        var userEmail = UserDetails.Email;
        var to_email = new EmailAddress(userEmail);
        var bookingID = bookingModel.Id;
        // var totalPrice = bookingModel.totalPrice;
        var plainTextContent = "Booking Success! please wait if your booking was accepted through the same email!" 
                               + Environment.NewLine 
                               + Environment.NewLine
                               + "Your Booking ID is: " + bookingID
                               + Environment.NewLine 
                               + "You can serve it as a receipt if anything goes wrong." 
                               + Environment.NewLine 
                               + Environment.NewLine 
                               + "Please wait for the confirmation email."
                               + Environment.NewLine
                               + Environment.NewLine
                               + "Thank you for choosing YourStudio!"; ;
        var msg = MailHelper.CreateSingleEmail(from_email, to_email, subject, plainTextContent, "");
        var response = await client.SendEmailAsync(msg).ConfigureAwait(false);
        
        //admin side email
        var apiKeyAdmin = "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";
        var clientAdmin = new SendGridClient(apiKeyAdmin);
        var from_emailAdmin = new EmailAddress("yourstudio.bacoor@gmail.com", "YourStudioSystem");
        var to_emailAdmin = new EmailAddress("yourstudio.bacoor@gmail.com");
        var subjectAdmin = "Booking Notification";
        var htmlContent = "<div style='background-image:url(https://i.imgur.com/ak8FrvS.png);padding:20px;text-align:center;list-style-type:none;font-family:\"Libre Franklin\"'><img src=https://i.imgur.com/Grjb8On.png style=height:30%><h1 style=color:#343a40>New Booking has been made!</h1><h1>Booking Details:</h1><h3>Booking ID: " + bookingID + "</h3><h3>Customer Email: " + userEmail + "</h3><h3>Booking Date: " + bookingModel.date + "</h3><h3>Booking Time: " + bookingModel.time + "</h3><h3>Booking Package: " + bookingModel.packages + "</h3><h3>Booking Add-ons: " + bookingModel.addOns + "</h3></div>";
        var plainTextContentAdmin = "New Booking has been made! Please check the admin panel for more details."
                                    + Environment.NewLine
                                    + "Booking Details: "
                                    + Environment.NewLine
                                    + "Booking ID: " + bookingID
                                    + Environment.NewLine
                                    + "Customer Email: " + userEmail
                                    + Environment.NewLine
                                    + "Booking Date: " + bookingModel.date
                                    + Environment.NewLine
                                    + "Booking Time: " + bookingModel.time
                                    + Environment.NewLine
                                    + "Booking Package: " + bookingModel.packages
                                    + Environment.NewLine
                                    + "Booking Add-ons: " + bookingModel.addOns;
                                    var msgAdmin = MailHelper.CreateSingleEmail(from_emailAdmin, to_emailAdmin, subject, "", htmlContent);
        var responseAdmin = await clientAdmin.SendEmailAsync(msgAdmin).ConfigureAwait(false);
        return RedirectToAction("Index");
    }
    
    //Delete Booking Function
    public async Task<IActionResult> DeleteBooking(string Id)
    {
        var bookingmodel = _context.Booking.FirstOrDefault(x => x.Id == Id);
        if (bookingmodel != null)
        {
            _context.Remove(bookingmodel);
            _context.SaveChanges();
        }

        return RedirectToAction("BList");
    }

    public async Task<IActionResult> submitPayment([FromForm(Name = "image")] IFormFile image, string Id)
    {
        var booking = _context.Booking.FirstOrDefault(x => x.Id == Id);
        if (booking != null)
        {
            var UserDetails = await _userManager.GetUserAsync(User);
            var fileName = Path.GetFileName(image.FileName);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", fileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
            var payment = new PaymentModel();
            payment.fileName = fileName;
            payment.Booking = booking;
            booking.payment = payment;
            booking.paymentID = payment.Id;
            booking.status = "DP_Paid";
            _context.Add(payment);
            _context.Update(booking);
            _context.SaveChanges();
            
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from_email = new EmailAddress("yourstudio.bacoor@gmail.com", "YourStudio");
            var subject = "Booking Confirmation";
            var email = UserDetails.Email;
            var to_email = new EmailAddress(email);
            var plainTextContent = "You have successfully paid your booking!"
                                   + Environment.NewLine
                                   + Environment.NewLine
                                   + "Your scheduled appointment is " + booking.date + " at " + booking.time + "."
                                   + Environment.NewLine
                                   + "Hope to see you there! "
                                   + Environment.NewLine
                                   + "Thank you for choosing YourStudio!";
            var msg = MailHelper.CreateSingleEmail(from_email, to_email, subject, plainTextContent, "");
            var response = await client.SendEmailAsync(msg).ConfigureAwait(false);
        }
        return RedirectToAction("Payment");
    }
    
    // //Edit Booking Function
    // public async Task<IActionResult> EditBooking(string Id)
    // {
    //     var bookingmodel = _context.Booking.FirstOrDefault(x => x.Id == Id);
    //     if (bookingmodel != null)
    //     {
    //         return View(bookingmodel);
    //     }
    //
    //     return RedirectToAction("BList");
    // }
    
    [Authorize(Roles = "Admin")]
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
            .Where(x => x.status == "Pending")
            .OrderByDescending(x => x.dateCreated).ToList());
    }
    
    
    //Accept Booking Function
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> acceptBooking(string Id, string email)
    {
        var bookingmodel = _context.Booking.FirstOrDefault(x => x.Id == Id);
        if (bookingmodel != null)
        {
            bookingmodel.status = "Accepted";
            _context.Update(bookingmodel);
            _context.SaveChanges();
            var getDate = _context.Booking.Where(x => x.status == "Pending").Where(x => x.date == bookingmodel.date).Include(x => x.accountUser).ToList();
            foreach (var booking in getDate)
            {
                rejectBooking(booking.Id, booking.accountUser.Email);
            }
        }
        
        var apiKey = "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";
        var client = new SendGridClient(apiKey);
        var from_email = new EmailAddress("yourstudio.bacoor@gmail.com", "YourStudio");
        var subject = "Booking Confirmation";
        var to_email = new EmailAddress(email);
        var hostURL = new Uri($"{Request.Scheme}://{Request.Host}/Booking/Payment");
        var plainTextContent = "Your Booking has been Accepted! Please upload your payment receipt to confirm your booking." 
                               + Environment.NewLine
                               + Environment.NewLine
                               + "Here is the link to upload your payment receipt: "
                               + hostURL
                               + Environment.NewLine
                               + Environment.NewLine
                               + "Thank you for choosing YourStudio!";
        var msg = MailHelper.CreateSingleEmail(from_email, to_email, subject, plainTextContent, "");
        var response = await client.SendEmailAsync(msg).ConfigureAwait(false);
        return RedirectToAction("BList");
    }

    public async Task<IActionResult> changeStatusToPaid(string Id, string email)
    {
        var bookingmodel = _context.Booking.FirstOrDefault(x => x.Id == Id);
        if (bookingmodel != null)
        {
            bookingmodel.status = "Paid";
            _context.Update(bookingmodel);
            _context.SaveChanges();
        }

        return RedirectToAction("BookingListAdminAccepted", "Booking");
    }


    //Reject Booking Function
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> rejectBooking(string Id, string email)
    {
        var bookingmodel = _context.Booking.FirstOrDefault(x => x.Id == Id);
        if (bookingmodel != null)
        {
            bookingmodel.status = "Rejected";
            _context.Update(bookingmodel);
            _context.SaveChanges();
        }
        
        var apiKey = "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";
        var client = new SendGridClient(apiKey);
        var from_email = new EmailAddress("yourstudio.bacoor@gmail.com", "YourStudio");
        var subject = "Booking Confirmation";
        var to_email = new EmailAddress(email);
        var plainTextContent = "Your Booking has been Declined..."
                               + Environment.NewLine
                               + Environment.NewLine
                               + "Womp womp"
                               + Environment.NewLine
                               + "Thank you for choosing YourStudio!";
        var msg = MailHelper.CreateSingleEmail(from_email, to_email, subject, plainTextContent, "");
        var response = await client.SendEmailAsync(msg).ConfigureAwait(false);
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
        
        return View(_context.Booking.Include(x => x.accountUser).Include(x => x.payment).Where(x => x.status == "Pending").ToList()); 

    }

    [Authorize(Roles = "User, Admin")]
    public async Task<IActionResult> BookingAll()
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
        
        return View(_context.Booking.Include(x => x.accountUser).Include(x => x.payment).ToList());
    }

    [Authorize(Roles = "User, Admin")]
    public async Task<ActionResult> BookingAccept()
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
        
        return View(_context.Booking.Include(x => x.accountUser).Include(x => x.payment).Where(x => x.status == "Accepted").ToList());
    }
    
    [Authorize(Roles = "User, Admin")]
    public async Task<ActionResult> BookingDecline()
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
        
        return View(_context.Booking.Include(x => x.accountUser).Include(x => x.payment).Where(x => x.status == "Rejected").ToList());
    }
    
    [Authorize(Roles = "User, Admin")]
    public async Task<ActionResult> BookingPaid()
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
        
        return View(_context.Booking.Include(x => x.accountUser).Include(x => x.payment).Where(x => x.status == "Paid").ToList());
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> BookingListAdmin()
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
        
        return View(_context.Booking.Include(x => x.accountUser).Include(x => x.payment).Where(x => x.status == "Paid").ToList());
    }
    
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> BookingListAdminAccepted()
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
        
        return View(_context.Booking.Include(x => x.accountUser).Include(x => x.payment).Where(x => x.status == "Accepted" || x.status == "DP_Paid").ToList());
    }
    
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> BookingListAdminDecline()
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
        
        return View(_context.Booking.Include(x => x.accountUser).Include(x => x.payment).Where(x => x.status == "Rejected").ToList());
    }

    public async Task<ActionResult> DeletedBooking(string ids)
    {
        var idList = ids.Split(",");
        foreach (var id in idList)
        {
            var bookingmodel = _context.Booking.FirstOrDefault(x => x.Id == id);
            if (bookingmodel != null)
            {
                _context.Remove(bookingmodel);
                _context.SaveChanges();
            }
        }

        return RedirectToAction("Booking", "Booking");
    }
    
    public async Task<ActionResult> DeletedBookingAdmin(string ids)
    {
        var idList = ids.Split(",");
        foreach (var id in idList)
        {
            var bookingmodel = _context.Booking.FirstOrDefault(x => x.Id == id);
            if (bookingmodel != null)
            {
                _context.Remove(bookingmodel);
                _context.SaveChanges();
            }
        }

        return RedirectToAction("BookingListAdmin", "Booking");
    }
}