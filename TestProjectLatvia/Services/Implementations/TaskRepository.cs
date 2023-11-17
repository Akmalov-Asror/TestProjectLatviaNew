using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Threading.Tasks;
using TestProjectLatvia.Data;
using TestProjectLatvia.Domains;
using TestProjectLatvia.Services.Interfaces;
using Task = TestProjectLatvia.Domains.Task;
using Tasks = System.Threading.Tasks.Task;

namespace TestProjectLatvia.Services.Implementations;

public class TaskRepository : ITaskRepository
{
    private readonly AppDbContext _context;

    public TaskRepository(AppDbContext context) => _context = context;

    public async Task<Domains.Task> GetTaskByIdAsync(int taskId) => await _context.Tasks.FirstOrDefaultAsync(x => x.Id == taskId);

    public async Task<List<Domains.Task>> GetAllTasksAsync() => await _context.Tasks.ToListAsync();

    public async Tasks CreateTaskAsync(Domains.Task task, ClaimsPrincipal claim)
    {
        var _check = await _context.Users.FirstOrDefaultAsync(x => x.UserName == claim.Identity.Name);

        var newTask = new Domains.Task
        {
            Title = task.Title,
            Description = task.Description,
            DueDate = task.DueDate,
            Status = task.Status
        };
        if (_check.Tasks is null)
        {
            _check.Tasks = new List<Domains.Task> {newTask };
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
}