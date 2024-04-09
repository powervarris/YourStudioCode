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
        return View();
    }
    public async Task<IActionResult> Otipi()
    {
        return View();
    }

    public IActionResult ResetPassword()
    {
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
            var subject = "Reset Password Confirmation";
            var to_email = new EmailAddress(email);
            var otp = new Random().Next(100000, 999999);
            var plainTextContent = "Enter the otp given in this email to reset your password " + otp;
            var msg = MailHelper.CreateSingleEmail(from_email, to_email, subject, plainTextContent, "");
            var response = await client.SendEmailAsync(msg).ConfigureAwait(false);
            TempData["otp"] = otp;
            TempData["email"] = email;
            return RedirectToAction("Otipi");
        }

        return RedirectToAction("Forget");
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
        if (otp == (int)TempData["otp"])
        {
            return RedirectToAction("ResetPassword");
        }

        return RedirectToAction("Forget");
    }

}