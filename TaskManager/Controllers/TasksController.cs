using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.Models;

[Authorize]
public class TasksController : Controller
{
    private readonly AppDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public TasksController(AppDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        if (User.IsInRole("Admin"))
            return View(await _context.Tasks.Include(t => t.User).ToListAsync());

        var userId = _userManager.GetUserId(User);
        return View(await _context.Tasks.Where(t => t.UserId == userId).ToListAsync());
    }

    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create(TaskItem task)
    {
        if (!ModelState.IsValid) return View(task);

        task.UserId = _userManager.GetUserId(User);
        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null) return NotFound();
        return View(task);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(TaskItem task)
    {
        if (!ModelState.IsValid) return View(task);

        _context.Tasks.Update(task);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null) return NotFound();

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}
