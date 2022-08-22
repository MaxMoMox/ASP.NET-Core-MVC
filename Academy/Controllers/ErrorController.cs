using Microsoft.AspNetCore.Mvc;

namespace Academy.Controllers;

public class ErrorController : Controller
{
    [HttpGet]
    public IActionResult Error()
    {
        return View();
    }
}