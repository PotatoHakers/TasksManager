using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Models;
using TaskManager.Data;

public class AccountController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly AppDbContext _context;

    public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, AppDbContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
    }

    [HttpGet]
    public IActionResult Register() => View();

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var user = new IdentityUser { UserName = model.Email, Email = model.Email };
        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index", "Tasks");
        }

        foreach (var error in result.Errors)
            ModelState.AddModelError("", error.Description);

        return View(model);
    }

    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {

        var log = new LoginLog
        {
            UserEmail = model.Email,
            LoginTime = DateTime.Now,
            IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString()
        };

        _context.LoginLogs.Add(log);
        await _context.SaveChangesAsync();

        if (!ModelState.IsValid) return View(model);

        var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
        if (result.Succeeded) return RedirectToAction("Index", "Tasks");

        ModelState.AddModelError("", "Неверный логин или пароль");
        return View(model);

    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        var email = User.Identity.Name;

        var lastLog = _context.LoginLogs
            .Where(l => l.UserEmail == email && l.LogoutTime == null)
            .OrderByDescending(l => l.LoginTime)
            .FirstOrDefault();

        if (lastLog != null)
        {
            lastLog.LogoutTime = DateTime.Now;
            await _context.SaveChangesAsync();
        }

        await _signInManager.SignOutAsync();
        return RedirectToAction("Login");

    }
}
