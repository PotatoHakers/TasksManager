using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.Models;
using TaskManager.Services;


namespace TaskManager.Controllers
{
    [Authorize]
    public class TasksController : Controller
    {
        private readonly TaskService _taskService;
        private readonly UserManager<IdentityUser> _userManager;
        public TasksController(TaskService taskService, UserManager<IdentityUser> userManager)
        {
            _taskService = taskService;
            _userManager = userManager;
        }
        //Read
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge(); // если не вошёл, редирект на login

            var tasks = await _taskService.GetUserTasksAsync(user.Id);
            return View(tasks);
        }
        //Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TaskItem task)
        {
            if (!ModelState.IsValid) return View(task);

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            task.UserId = user.Id;
            await _taskService.AddAsync(task);
            return RedirectToAction(nameof(Index));
        }

        //Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var task = await _taskService.GetByIdAsync(id);
            if (task == null) return NotFound();
            return View(task);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(TaskItem task)
        {
            if (!ModelState.IsValid)
                return View(task);

            await _taskService.UpdateAsync(task);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var task = await _taskService.GetByIdAsync(id);
            if (task == null) return NotFound();
            return View(task);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _taskService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
