using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TestProjectLatvia.Data;
using TestProjectLatvia.Domains;
using TestProjectLatvia.Services.Interfaces;
using Task = TestProjectLatvia.Domains.Task;
using Tasks = System.Threading.Tasks.Task;

namespace TestProjectLatvia.Services.Implementations;

public class TaskRepository : ITaskRepository
{
    private readonly AppDbContext _context;
    private readonly UserManager<User> _userManager;
    public TaskRepository(AppDbContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<Domains.Task> GetTaskByIdAsync(int taskId) => await _context.Tasks.FirstOrDefaultAsync(x => x.Id == taskId);

    public async Task<List<Domains.Task>> GetAllTasksAsync() => await _context.Tasks.ToListAsync();

    public async Tasks CreateTaskAsync(Domains.Task task, string email)
    {
        var _check = await _context.Users.Include(x => x.Tasks).FirstOrDefaultAsync(x => x.Email == email);
        var newTask = new Domains.Task
        {
            Title = task.Title,
            Description = task.Description,
            DueDate = task.DueDate,
            Status = task.Status
        };
        if (_check.Tasks is null)
        {
            _check.Tasks = new List<Domains.Task> { newTask };
        }
        else
        {
            _check.Tasks.Add(newTask);
        }

        _context.Tasks.Add(newTask);
        await _context.SaveChangesAsync();
    }

    public async Task<Task> UpdateTaskAsync(Domains.Task task)
    {
        var existingTask = await _context.Tasks.FirstOrDefaultAsync(x => x.Id == task.Id);

        if (existingTask != null)
        {
            existingTask.Title = task.Title;
            existingTask.Description = task.Description;
            existingTask.DueDate = task.DueDate;
            existingTask.Status = task.Status;

            await _context.SaveChangesAsync();
        }

        return existingTask;
    }
    public async Task<Task> GetOldValueAsync(int id) => await _context.Tasks.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
    public async Task<Task> DeleteTaskAsync(int taskId)
    {
        var currentProduct = await _context.Tasks.FirstOrDefaultAsync(x => x.Id == taskId);
        if (currentProduct == null) throw new Exception("Product not found");
        _context.Tasks.Remove(currentProduct);
        await _context.SaveChangesAsync();
        return currentProduct;

    }
    public async Task<Task> CreateAudit(Task newProduct, Task oldProduct, string actionType, User user)
    {
        var auditTrailRecord = new AuditLog
        {
            UserName = user.UserName,
            Action = actionType,
            ControllerName = "Task",
            DateTime = DateTime.UtcNow,
            OldValue = JsonConvert.SerializeObject(oldProduct, Formatting.Indented),
            NewValue = JsonConvert.SerializeObject(newProduct, Formatting.Indented)
        };

        _context.AuditLog.Add(auditTrailRecord);
        try
        {
            await _context.SaveChangesAsync();
            return newProduct;
        }
        catch (Exception ex)
        {
            throw new Exception("Error saving audit log.", ex);
        }
    }

    public async Task<Task> CheckTaskName(Task task)
    {
        var checkBase = await _context.Tasks.FindAsync(task);
        return checkBase ?? new Task();
    }

    public async Task<User> AddTaskForUser(string email, string taskName)
    {
        var check = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
        var checkTask = await _context.Tasks.FirstOrDefaultAsync(x => x.Title == taskName);
        if (check.Tasks == null)
        {
            check.Tasks = new List<Task>{checkTask};
        }
        else
        {
            check.Tasks.Add(checkTask);
        }

        await _context.SaveChangesAsync();
        return check;   
    }
}