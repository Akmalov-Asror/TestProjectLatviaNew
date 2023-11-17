using System.Globalization;
using Microsoft.EntityFrameworkCore;
using TestProjectLatvia.Data;
using TestProjectLatvia.Domains;
using TestProjectLatvia.ExtensionFunctions;
using TestProjectLatvia.Services.Interfaces;
using TestProjectLatvia.ViewModels;

namespace TestProjectLatvia.Services.Implementations;

public class AuditRepository : IAuditRepository
{
    private readonly AppDbContext _context;

    public AuditRepository(AppDbContext context) => _context = context;

    public async Task<AuditLogViewModel> Index(DateTime? fromDate, DateTime? toDate, string Name)
    {
        var auditLogs = await _context.AuditLog.ToListAsync();

        var filteredLogs = ForAudit.FilterAuditLogsByDate(auditLogs, fromDate, toDate, Name);

        var viewModel = new AuditLogViewModel
        {
            FromDate = fromDate ?? DateTime.Today.AddDays(-100),
            ToDate = toDate ?? DateTime.Today,
            FilteredLogs = filteredLogs
        };

        return viewModel;
    }

    public async Task<List<AuditLog>> SortByUserName(string name)
    {
        var auditLogs = _context.AuditLog
            .AsEnumerable()
            .Where(log => log.UserName.Equals(name, StringComparison.OrdinalIgnoreCase))
            .ToList();

        return auditLogs;
    }

    public async Task<List<AuditLog>> GetFiltered(string? fromDate, string? toDate)
    {
        var dateFormat = "dd.MM.yyyy";


        if (!DateTime.TryParseExact(fromDate, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out var fromDateParsed))
        {
            if (fromDate != null)
                throw new Exception("Invalid date format. fromDate For example : dd.mm.yyyy");
        }

        if (!DateTime.TryParseExact(toDate, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out var toDateParsed))
        {
            if (toDate != null)
                throw new Exception("Invalid date format. toDate For example : dd.mm.yyyy");
        }

        fromDateParsed = DateTime.SpecifyKind(fromDateParsed, DateTimeKind.Utc);
        toDateParsed = DateTime.SpecifyKind(toDateParsed, DateTimeKind.Utc);


        if (toDate != null)
        {
            if (fromDateParsed.Date > toDateParsed.Date)
                throw new Exception("To Date cannot be before From Date.");
        }

        var auditLogs = await _context.AuditLog
            .Where(log =>
                (fromDateParsed == DateTime.MinValue || log.DateTime >= fromDateParsed) &&
                (toDateParsed == DateTime.MinValue || log.DateTime <= toDateParsed))
            .ToListAsync();

        return auditLogs;
    }

    public async Task<List<AuditLog>> GetAllAudits()
    {
        var auditLogs = await _context.AuditLog.ToListAsync();
        return auditLogs;
    }
}