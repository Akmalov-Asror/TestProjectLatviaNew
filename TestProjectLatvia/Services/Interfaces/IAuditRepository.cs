using TestProjectLatvia.Domains;
using TestProjectLatvia.ViewModels;

namespace TestProjectLatvia.Services.Interfaces;

public interface IAuditRepository
{
    /// <summary>
    /// Retrieves audit log data based on date range and user name.
    /// </summary>
    /// <param name="fromDate">Start date of the filter range (optional).</param>
    /// <param name="toDate">End date of the filter range (optional).</param>
    /// <param name="name">Name of the user for filtering (optional).</param>
    /// <returns>Returns a Task containing AuditLogViewModel data based on the specified criteria.</returns>
    /// <example>
    /// <code>
    /// var fromDate = new DateTime(2023, 01, 01);
    /// var toDate = new DateTime(2023, 12, 31);
    /// var userName = "JohnDoe";
    /// 
    /// // Usage example:
    /// var auditLogs = await auditLogService.Index(fromDate, toDate, userName);
    /// foreach (var log in auditLogs)
    /// {
    ///     Console.WriteLine($"Action: {log.Action}, Timestamp: {log.Timestamp}");
    /// }
    /// </code>
    /// </example>
    Task<AuditLogViewModel> Index(DateTime? fromDate, DateTime? toDate, string name);

    /// <summary>
    /// Retrieves sorted audit log data based on user name.
    /// </summary>
    /// <param name="name">Name of the user for sorting.</param>
    /// <returns>Returns a Task containing a List of AuditLog items sorted by user name.</returns>
    Task<List<AuditLog>> SortByUserName(string name);

    /// <summary>
    /// Retrieves audit log data based on a date range.
    /// </summary>
    /// <param name="fromDate">Start date of the filter range (optional).</param>
    /// <param name="toDate">End date of the filter range (optional).</param>
    /// <returns>Returns a Task containing a List of AuditLog items filtered by the specified date range.</returns>
    Task<List<AuditLog>> GetFiltered(string? fromDate, string? toDate);

    /// <summary>
    /// Retrieves all audit log data.
    /// </summary>
    /// <returns>Returns a Task containing a List of all available AuditLog items.</returns>
    Task<List<AuditLog>> GetAllAudits();
}