using System.Security.Claims;
using TestProjectLatvia.Domains;
using TestProjectLatvia.ViewModels;
using Task = System.Threading.Tasks.Task;

namespace TestProjectLatvia.Services.Interfaces;

public interface ITaskRepository
{
    Task<Domains.Task> GetTaskByIdAsync(int taskId);
    Task<List<Domains.Task>> GetAllTasksAsync();
    Task CreateTaskAsync(Domains.Task task, ClaimsPrincipal claim);
    Task<Domains.Task> UpdateTaskAsync(Domains.Task task);
    Task<Domains.Task> DeleteTaskAsync(int taskId);
    public Task<Domains.Task> GetOldValueAsync(int id);
    /// <summary>
    /// Creates an audit log entry for a product operation asynchronously.
    /// </summary>
    /// <param name="entity">Product entity representing the current state of the product.</param>
    /// <param name="oldValue">Product entity representing the previous state of the product.</param>
    /// <param name="actionType">The type of action performed (e.g., 'Create', 'Update', 'Delete').</param>
    /// <param name="user">User object representing the user performing the action.</param>
    /// <returns>
    ///     Returns a Task representing the completion of the audit log creation.
    /// </returns>
    public Task<Domains.Task> CreateAudit(Domains.Task entity, Domains.Task oldValue, string actionType, User user);
}