using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using TestProjectLatvia.Domains;
using TestProjectLatvia.Services.Interfaces;
using TestProjectLatvia.ViewModels;
using Task = TestProjectLatvia.Domains.Task;

namespace TestProjectLatvia.Controllers;

public class TaskController : Controller
{
    private readonly ITaskRepository _taskRepository;
    private readonly UserManager<User> _userManager;
    private readonly IToastNotification _toastNotification;

    public TaskController(ITaskRepository productRepository, UserManager<User> userManager, IToastNotification toastNotification)
    {
        _taskRepository = productRepository;
        _userManager = userManager;
        _toastNotification = toastNotification;
    }

    public async Task<IActionResult> Index()
    {
        var tasks = await _taskRepository.GetAllTasksAsync();

        var productViewModels = tasks.Select(p => new Task()
        {
            Id = p.Id,
            Title = p.Title,
            Description = p.Description,
            DueDate = p.DueDate,
            Status = p.Status,
        }).ToList();

        return View(productViewModels);
    }
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> Details(int id)
    {
        try
        {
            if (id == null) return NotFound();

            var task = await _taskRepository.GetTaskByIdAsync(id);
            if (task == null) return NotFound();
            _toastNotification.AddSuccessToastMessage("Details Found!");
            return View(task);

        }
        catch (Exception ex)
        {
            _toastNotification.AddErrorToastMessage("Details Not Found!");
            return RedirectToAction("Index", "Home");
        }

    }
    [Authorize(Roles = "ADMIN, MANAGER")]
    public IActionResult Create() => View();

    [Authorize(Roles = "ADMIN,MANAGER")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Title,Description,DueDate,Status")] Task task)
    {
        if (!ModelState.IsValid) return View(task);
        task.DueDate = DateTime.SpecifyKind(task.DueDate, DateTimeKind.Utc);

        DateTime thresholdDate = new DateTime(2030, 1, 1);
        if (task.DueDate > thresholdDate)
        {
            _toastNotification.AddErrorToastMessage("You can enter until January 1, 2030!");
            return View(task);
        }
        var user = await _userManager.GetUserAsync(HttpContext.User);
        await _taskRepository.CreateTaskAsync(task);
        await _taskRepository.CreateAudit(task, null, "Create", user);
        _toastNotification.AddSuccessToastMessage("Created successfully!");
        return RedirectToAction(nameof(Index));
    }
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            if (id == null) return NotFound();

            var task = await _taskRepository.GetTaskByIdAsync(id);
            if (task == null) return NotFound();

            return View(task);
        }
        catch (Exception ex)
        {
            return RedirectToAction("Index", "Home");
        }
    }
    [Authorize(Roles = "ADMIN")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,DueDate,Status")] Task task)
    {
        if (id != task.Id)
            return NotFound();

        if (!ModelState.IsValid)
        {
            _toastNotification.AddErrorToastMessage("There are not enough items entered, please try again!");
            return View(task);
        }

        try
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var oldProduct = await _taskRepository.GetOldValueAsync(id);
            var newProduct = await _taskRepository.UpdateTaskAsync(task);
            await _taskRepository.CreateAudit(newProduct, oldProduct, "Edit", user);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (_taskRepository.GetTaskByIdAsync(task.Id) == null)
                return NotFound();
            else
                throw;
        }
        _toastNotification.AddSuccessToastMessage("task changed successfully!");
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            if (id == null) return NotFound();

            var product = await _taskRepository.GetTaskByIdAsync(id);

            if (product == null) return NotFound();
            
            return View(product);
        }
        catch
        {
            return RedirectToAction("Index", "Home");
        }

    }


    [Authorize(Roles = "ADMIN")]
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var task = await _taskRepository.DeleteTaskAsync(id);
        if (task == null) return NotFound();

        var user = await _userManager.GetUserAsync(HttpContext.User);
        await _taskRepository.CreateAudit(task, null, "Delete", user);
        _toastNotification.AddSuccessToastMessage("your models deleted!");
        return RedirectToAction(nameof(Index));
    }
}