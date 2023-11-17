using TestProjectLatvia.Domains;

namespace TestProjectLatvia.ExtensionFunctions;

public static class ForAudit
{
    public static List<AuditLog> FilterAuditLogsByDate(List<AuditLog> logs, DateTime? fromDate, DateTime? toDate, string Name)
    {
        var filteredLogs = logs
            .Where(log =>
                (!fromDate.HasValue || log.DateTime >= fromDate) &&
                (!toDate.HasValue || log.DateTime <= toDate?.AddDays(1)) &&
                (Name == null || log.UserName.IndexOf(Name, StringComparison.OrdinalIgnoreCase) >= 0)
            )
            .ToList();
        return filteredLogs;
    }
}