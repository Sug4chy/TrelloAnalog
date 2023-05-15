using Microsoft.AspNetCore.Mvc;

namespace Learning_ASP.NET.Controllers;

public class HomeController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Index(string userName, string day)
    {
        var date = DateTime.Parse(day);
        return userName.Equals("Stepan") 
            ? RedirectToAction("GetActionsByDay", "ToDo", new { day = date }) : View();
    }
}