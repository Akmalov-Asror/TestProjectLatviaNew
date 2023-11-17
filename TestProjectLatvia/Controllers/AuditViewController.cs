using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestProjectLatvia.Services.Interfaces;

namespace TestProjectLatvia.Controllers;
[Authorize(Roles = "ADMIN")]
public class AuditViewController : Controller
{
    private readonly IAuditRepository _auditRepository;
    public AuditViewController(IAuditRepository context) => _auditRepository = context;
    public async Task<IActionResult> Index(DateTime? fromDate, DateTime? toDate, string Name) => View(await _auditRepository.Index(fromDate, toDate, Name));

}