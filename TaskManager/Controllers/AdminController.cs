using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Data;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly AppDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public AdminController(AppDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Dashboard()
    {
        ViewBag.TasksTotal = await _context.Tasks.CountAsync();
        ViewBag.UsersTotal = await _userManager.Users.CountAsync();
        return View();
    }

    public async Task<IActionResult> Users() => View(await _userManager.Users.ToListAsync());

    public async Task<IActionResult> Logs() => View(await _context.LoginLogs.OrderByDescending(l => l.LoginTime).ToListAsync());
}
