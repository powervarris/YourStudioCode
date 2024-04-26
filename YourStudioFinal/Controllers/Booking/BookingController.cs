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
        var htmlContent2 = "<div style=background-image:url(https://i.imgur.com/ak8FrvS.png);padding:20px;text-align:center;list-style-type:none><img src=https://i.imgur.com/Grjb8On.png style=height:30%><h1 style=color:#343a40>New Booking has been made!</h1><h4>We hope this message find you well.</h4><br><h4>We wish to inform you that your recent booking with us is currently pending confirmation. Please be assured that our team is diligently working to finalize your booking.</h4><br><h4>We kindly ask for your patience during the process. Rest assured, we will notify you promptly once your booking is confirmed.</h4><br><h1>Booking Details:</h1><h3>Booking ID: " + bookingID + "</h3><h3>Customer Email: " + userEmail + "</h3><h3>Booking Date: " + bookingModel.date + "</h3><h3>Booking Time: " + bookingModel.time + "</h3><h3>Booking Package: " + bookingModel.packages + "</h3><h3>Booking Add-ons: " + bookingModel.addOns + "</h3><h3>Best Regards!</h3><h3>Your Studio</h3></div>";
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
        var msg = MailHelper.CreateSingleEmail(from_email, to_email, subject, "", htmlContent2);
        var response = await client.SendEmailAsync(msg).ConfigureAwait(false);
        
        // admin side email
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
            booking.status = "RTP";
            _context.Add(payment);
            _context.Update(booking);
            _context.SaveChanges();
            
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from_emailAdmin = new EmailAddress("yourstudio.bacoor@gmail.com", "YourStudioSystem");
            var to_emailAdmin = new EmailAddress("yourstudio.bacoor@gmail.com");
            var subjectAdmin = "Booking Notification";
            var htmlContent4 = "<div style=background-image:url(https://i.imgur.com/ak8FrvS.png);padding:20px;text-align:center;list-style-type:none><img src=https://i.imgur.com/Grjb8On.png style=height:30%><h3>Hello,</h3><h3>A customer with the following service details has completed a payment transaction to their appointed booking.</h3><h3>Full name: " + UserDetails.Fname + UserDetails.Lname + " </h3><h3>Date: " + booking.date + " </h3><h3>Time: " + booking.time + " </h3><h3>Package: " + booking.packages + " </h3><h3>Add-Ons: " + booking.addOns + " </h3><h3>Total Price: " + booking.totalPrice + " </h3><h3>Down Payment: " + booking.downPayment + " </h3><br><h3>Please verify the reference code and validity of the receipt in the Booking List.</h3></div>";
            var plainTextContent = "You have successfully paid your booking!"
                                   + Environment.NewLine
                                   + Environment.NewLine
                                   + "Your scheduled appointment is " + booking.date + " at " + booking.time + "."
                                   + Environment.NewLine
                                   + "Hope to see you there! "
                                   + Environment.NewLine
                                   + "Thank you for choosing YourStudio!";
            var msg = MailHelper.CreateSingleEmail(from_emailAdmin, to_emailAdmin, subjectAdmin, "", htmlContent4);
            var response = await client.SendEmailAsync(msg).ConfigureAwait(false);
        }
        return RedirectToAction("Payment");
    }

    public async Task<IActionResult> changeToDP_Paid(string Id)
    {
        var UserDetails = await _userManager.GetUserAsync(User);
        var bookingmodel = _context.Booking.FirstOrDefault(x => x.Id == Id);
        if (bookingmodel != null)
        {
            bookingmodel.status = "DP_Paid";
            _context.Update(bookingmodel);
            _context.SaveChanges();

            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from_email = new EmailAddress("yourstudio.bacoor@gmail.com", "YourStudio");
            var subject = "Down Payment Received";
            var email = UserDetails.Email;
            var to_email = new EmailAddress(email);
            var htmlContent5 = "<div style=background-image:url(https://i.imgur.com/ak8FrvS.png);padding:20px;text-align:center;list-style-type:none><img src=https://i.imgur.com/Grjb8On.png style=height:30%><h3>Dear</h3><h3>We hope this message finds you well,</h3><h3>Your recent down payment have been successfully submitted. Your prompt action is greatly appreciated and serves as a testament to your commitment.</h3><h3>Your payment has been successfully processed, and we are pleased to confirm that your reservation is secured.</h3><h3>Once again, thank you for choosing YourStudio. We are honored to ahve the opportunity to serve you, and we are committed to making your expeirence with us memorable and enjoyable.</h3><br><br><h4>Thank you for you understanding and continued support.</h4><h4>Best Regards,</h4><h4>Your Studio</h4></div>";
            var plainTextContent = "";
            var msg = MailHelper.CreateSingleEmail(from_email, to_email, subject, "", htmlContent5);
            var response = await client.SendEmailAsync(msg).ConfigureAwait(false);
        }
        return RedirectToAction("BookingListAdminAccepted", "Booking");
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
        var htmlContent44 = "<div style=background-image:url(https://i.imgur.com/ak8FrvS.png);padding:20px;text-align:center;list-style-type:none><img src=https://i.imgur.com/Grjb8On.png style=height:30%><h3>Dear</h3><h3>We hope this message finds you well,</h3><h3>We are delighted to inform you that your booking with YourStudio has been successfully accepted! We look forward to welcome you on " + bookingmodel.date + " at " + bookingmodel.time + " for you scheduled services.</h3><h3>We kindly request that you proceed with the payment at your earliest convention. You are given time to pay minimum 500 down payment to ensure the settled booking. Note that if you have avail the YourStudent Package Deal you are required to pay a downpayment of 50%</h3><h3>Should you have any special request or require further assistance, please do not hesitate to reach out to our team.</h3><h3>Your satisfaction is our top priority, and we are committed to ensuring that your experience with us exceed your expectionations! Thank you for choosing YourStudio. We are honored to have the opportunity to serve you and look forward to prroviding you with an exceptional experience.</h3><h3>Here is the link of the payment " + hostURL + "</h3><br><br><h4>Thank you for you understanding and continued support.</h4><h4>Best Regards,</h4><h4>Your Studio</h4></div>";
        var plainTextContent = "Your Booking has been Accepted! Please upload your payment receipt to confirm your booking." 
                               + Environment.NewLine
                               + Environment.NewLine
                               + "Here is the link to upload your payment receipt: "
                               + hostURL
                               + Environment.NewLine
                               + Environment.NewLine
                               + "Thank you for choosing YourStudio!";
        var msg = MailHelper.CreateSingleEmail(from_email, to_email, subject, "", htmlContent44);
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
    
    public async Task<IActionResult> changeStatusToReject(string Id, string email)
    {
        var bookingmodel = _context.Booking.FirstOrDefault(x => x.Id == Id);
        if (bookingmodel != null)
        {
            bookingmodel.status = "Rejected";
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
        
        var UserDetails = await _userManager.GetUserAsync(User);
        var email2 = UserDetails.Email;
        
        var apiKey = "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";
        var client = new SendGridClient(apiKey);
        var from_email = new EmailAddress("yourstudio.bacoor@gmail.com", "YourStudio");
        var subject = "Booking Update Status";
        var to_email = new EmailAddress(email2);
        var htmlContent3 = "<div style=background-image:url(https://i.imgur.com/ak8FrvS.png);padding:20px;text-align:center;list-style-type:none><img src=https://i.imgur.com/Grjb8On.png style=height:30%><h3>Dear, " + UserDetails.Fname + "</h3><h3>We hope this message finds you well,</h3><h3>We regret to inform you that your recent booking with us has unfortunately been declined. We sincerely apologize for any inconvenience it may have caused.</h3><h3>YourStudio has carefully reviewed your booking request, and unfortunately, we are unable to accommodate it.</h3><h3>If you have any questions or require further assistance, please do not hesitate to contact us. We are here to assist you in any way we can.</h3><br><br><h4>Thank you for you understanding and continued support.</h4><h4>Best Regards,</h4><h4>Your Studio</h4></div>";
        var plainTextContent = "Your Booking has been Declined..."
                               + Environment.NewLine
                               + Environment.NewLine
                               + "Womp womp"
                               + Environment.NewLine
                               + "Thank you for choosing YourStudio!";
        var msg = MailHelper.CreateSingleEmail(from_email, to_email, subject, "", htmlContent3);
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
            TempData["Error"] = "You need to login to access this page";
            return RedirectToAction("Index", "Account");
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
        
        return View(_context.Booking.Include(x => x.accountUser).Include(x => x.payment).Where(x => x.status == "Paid" || x.status == "DP_Paid").ToList());
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
        
        return View(_context.Booking.Include(x => x.accountUser).Include(x => x.payment).Where(x => x.status == "Accepted" || x.status == "DP_Paid" || x.status == "RTP").ToList());
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