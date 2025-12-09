using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Data;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly AppDbContext _context;

    public AdminController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Logs()
    {
        var logs = await _context.LoginLogs
            .OrderByDescending(l => l.LoginTime)
            .ToListAsync();

        return View(logs);
    }
}
