using Microsoft.AspNetCore.Mvc;

namespace TestProjectLatvia.Controllers;

public class NotFoundController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}