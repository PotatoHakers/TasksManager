using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.Models;
using TaskManager.Services;


namespace TaskManager.Controllers
{
    public class TasksController : Controller
    {
        private readonly TaskService _taskService;
        public TasksController(TaskService taskService)
        {
            _taskService = taskService;
        }
        //Read
        public async Task<IActionResult> Index(string? search, bool? showDone)
        {
            var tasks = await _taskService.GetAllTasksAsync(); // получаем данные из БД

            if (!string.IsNullOrWhiteSpace(search))
                tasks=tasks.Where(t => t.Title.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();

            if (showDone.HasValue)
                tasks = tasks.Where(t => t.IsDone == showDone.Value).ToList();

            ViewBag.Search = search;
            ViewBag.ShowDone = showDone;

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
            if (ModelState.IsValid)
            return View(task);

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
