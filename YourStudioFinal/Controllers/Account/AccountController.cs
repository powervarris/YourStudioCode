using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SendGrid;
using SendGrid.Helpers.Mail;
using YourStudioFinal.Models;
using YourStudioFinal.data;

namespace YourStudioFinal.Controllers.Account;

[AllowAnonymous]
public class AccountController : Controller
{
    private readonly ILogger<AccountController> _logger;

    private readonly ApplicationDbContext _context;

    private readonly UserManager<User> _userManager;

    private readonly SignInManager<User> _signInManager;
    public AccountController(ILogger<AccountController> logger, ApplicationDbContext context, UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<IActionResult> Index()
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

    public async Task<IActionResult> Register()
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
    public IActionResult Forget()
    {
        ViewBag.isLogged = false;
        return View();
    }
    public async Task<IActionResult> Otipi()
    {
        ViewBag.isLogged = false;
        return View();
    }
    public async Task<IActionResult> verifyEmail()
    {
        ViewBag.isLogged = false;
        return View();
    }
    public IActionResult ResetPassword()
    {
        ViewBag.isLogged = false;
        return View();
    }
    public IActionResult Terms()
    {
        return View();
    }


    [HttpPost]
    public async Task<IActionResult> AddUser(User usermodel)
    {
        usermodel.dateCreated = DateTime.Now;
        PasswordHasher<User> passwordHasher = new PasswordHasher<User>();
        usermodel.PasswordHash = passwordHasher.HashPassword(usermodel, usermodel.password);
        await _userManager.CreateAsync(usermodel);
        await _userManager.AddToRoleAsync(usermodel, "User");
        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]

    public async Task<IActionResult> loginUser(User usermodel)
    {
        var loginResult = await _signInManager.PasswordSignInAsync(usermodel.UserName, usermodel.password, false, false);
        if (loginResult.Succeeded)
        {
            return RedirectToAction("Index", "Home");

        }
        else
        {
            ViewBag.isLogged = false;
            ModelState.AddModelError(string.Empty, "Invalid username or password.");
            TempData["Error"] = "Invalid username or password.";
        }
        return View("Index", "Account");
    }

    // public async Task<IActionResult> Logout()
    // {
    //     await _signInManager.SignOutAsync();
    //     return RedirectToAction("Index", "Home");
    // }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public async Task<IActionResult> validateEmail(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user != null)
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from_email = new EmailAddress("yourstudio.bacoor@gmail.com", "YourStudio");
            var subject = "YourStudio Forgot Password Verification OTP";
            var to_email = new EmailAddress(email);
            var otp = new Random().Next(100000, 999999);
            var htmlcontent = "<div style=background-image:url(https://i.imgur.com/ak8FrvS.png);padding:20px;text-align:center;list-style-type:none><img src=https://i.imgur.com/Grjb8On.png style=height:30%><h3>Dear, " + user.Fname + "</h3><h3>We noticed that you’re having trouble logging in. No worries, with the help of YourStudio you can regain access to your account. Please use the following One-Time Password (OTP) to verify your identity and reset your forgotten password.</h3><br><br><br><h3>" + otp + "is your YourStudio Forgot Password Verification Code.</h3><br><h4>Enter this OTP on the password reset page to proceed.</h4><br><br><br><br><h4>Thank you!</h4><h4>Best Regards,</h4><h4>Your Studio</h4>";
            var plainTextContent = "Enter the otp given in this email to reset your password " + otp;
            var msg = MailHelper.CreateSingleEmail(from_email, to_email, subject, "", htmlcontent);
            var response = await client.SendEmailAsync(msg).ConfigureAwait(false);
            TempData["otp"] = otp;
            TempData["email"] = email;
            return RedirectToAction("Otipi");
        }

        return RedirectToAction("Forget");
    }

    public async Task<IActionResult> validateAccount(string Email, string UserName, string password, string mobileNumber, string Fname, string Lname)
    {
        var user = await _userManager.FindByEmailAsync(Email);

        if (Email != null)
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from_email = new EmailAddress("yourstudio.bacoor@gmail.com", "YourStudio");
            var subject = "Register Account Verification Confirmation";
            var to_email = new EmailAddress(Email);
            var otp = new Random().Next(100000, 999999);
            var htmlContent10 = "<div style=background-image:url(https://i.imgur.com/ak8FrvS.png);padding:20px;text-align:center;list-style-type:none><img src=https://i.imgur.com/Grjb8On.png style=height:30%><h3>Hello,</h3><h3>"+ otp +" is your YourStudio Registration verification code</h3><h3>Enter the code above into the system to finish the registration process.</h3><h3>IMPORTANT: Don’t share your security codes with anyone. YourStudio will never ask for your codes. This can include things like texting your code, screen sharing, etc. By sharing your security codes with someone else, you are putting your account and its content at high risk.</h3><br><h4>Best Regards,</h4><h4>Your Studio</h4></div>";
            var plainTextContent = "Enter the otp given in this email to confirm your account " + otp;
            var msg = MailHelper.CreateSingleEmail(from_email, to_email, subject, "", htmlContent10);
            var response = await client.SendEmailAsync(msg).ConfigureAwait(false);
            TempData["Fname"] = Fname;
            TempData["Lname"] = Lname;
            TempData["Email"] = Email;
            TempData["UserName"] = UserName;
            TempData["password"] = password;
            TempData["mobileNumber"] = mobileNumber;
            TempData["otp"] = otp;
            return RedirectToAction("verifyEmail");
        }

        return RedirectToAction("Register");
    }

    public async Task<IActionResult> verifyAccountOtp(int otp, User usermodel)
    {
        if (otp != (int)TempData["otp"])
        {
            TempData["OtpError"] = "The OTP entered does not match. Please try again.";
            TempData.Keep();
            return RedirectToAction("verifyEmail");
        }

        await AddUser(usermodel);

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> changePassword(string password, string confirmpassword)
    {
        var user = await _userManager.FindByEmailAsync(TempData["email"].ToString());
        if (user != null)
        {
            if (password == confirmpassword)
            {
                PasswordHasher<User> passwordHasher = new PasswordHasher<User>();
                user.PasswordHash = passwordHasher.HashPassword(user, password);
                await _userManager.UpdateAsync(user);
                return RedirectToAction("Index");
            }
            return RedirectToAction("ResetPassword");
        }

        return RedirectToAction("Forget");
    }

    public async Task<IActionResult> verifyOtp(int otp)
    {
        if (otp != (int)TempData["otp"])
        {
            TempData["verifyOtpError"] = "The OTP entered does not match. Please try again.";
            return RedirectToAction("Otipi");
        }

        return RedirectToAction("ResetPassword");
    }

}