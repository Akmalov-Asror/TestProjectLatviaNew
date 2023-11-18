using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TestProjectLatvia.Domains;
using TestProjectLatvia.Services.Interfaces;
using Tasks = TestProjectLatvia.Domains.Task;

namespace TestProjectLatvia.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "ADMIN")]
public class TaskApiController : ControllerBase
{
    private readonly ITaskRepository _taskRepository;
    private readonly UserManager<User> _userManager;
    public TaskApiController(ITaskRepository taskRepository, UserManager<User> userManager)
    {
        _taskRepository = taskRepository;
        _userManager = userManager;
    }
    [HttpGet]
    public async Task<IActionResult> Index() => Ok(await _taskRepository.GetAllTasksAsync());

    [HttpPost]
    public async Task<IActionResult> Create(Tasks task, string email)
    {
        task.DueDate = DateTime.SpecifyKind(task.DueDate, DateTimeKind.Utc);

        DateTime thresholdDate = new DateTime(2030, 1, 1);
        var oldTime = DateTime.UtcNow;
        if (task.DueDate > thresholdDate && task.DueDate < oldTime)
        {
            throw new Exception("please enter again date");
        }
        var user = await _userManager.GetUserAsync(HttpContext.User);
        await _taskRepository.CreateTaskAsync(task, email);
        await _taskRepository.CreateAudit(task, null, "Create", user);
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> Edit(int id,  Tasks task)
    {
        if (id != task.Id)
            return NotFound();
        var user = await _userManager.GetUserAsync(HttpContext.User);
        var oldTask = await _taskRepository.GetOldValueAsync(id);
        var newTask = await _taskRepository.UpdateTaskAsync(task);
        await _taskRepository.CreateAudit(newTask, oldTask, "Edit", user);
        return Ok(newTask);
    }



    [HttpDelete]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var task = await _taskRepository.DeleteTaskAsync(id);
        if (task == null) return NotFound();

        var user = await _userManager.GetUserAsync(HttpContext.User);
        await _taskRepository.CreateAudit(task, null, "Delete", user);
        return Ok();
    }
}