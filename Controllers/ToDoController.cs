using System.Globalization;
using Learning_ASP.NET.Models;
using Learning_ASP.NET.Services;
using Microsoft.AspNetCore.Mvc;

namespace Learning_ASP.NET.Controllers;

public class ToDoController : Controller
{
    private const string FolderName = "ToDoSheets";
    
    [HttpGet]
    public IActionResult GetAllActions()
    {
        var allActions = new List<ToDoActionsSheet>();
        var folder = new DirectoryInfo(FolderName);
        foreach (var file in folder.GetFiles())
        {
            var actions = CsvParser.Parse<ToDoAction>($"{FolderName}//{file.Name}");
            var sheet = new ToDoActionsSheet(file.Name.Split(".csv")[0], actions);
            allActions.Add(sheet);
        }

        return View(allActions);
    }

    [HttpGet]
    public IActionResult GetActionsByDay(DateTime day)
    {
        var dayAdapted = day.ToString(CultureInfo.CurrentCulture).Split(' ')[0];
        var actions = CsvParser.Parse<ToDoAction>($"{FolderName}//{dayAdapted}.csv");
        return View(new ToDoActionsSheet(dayAdapted, actions));
    }

    [HttpGet]
    public IActionResult CreateToDoAction()
    {
        return View();
    }

    [HttpPost]
    public IActionResult CreateToDoAction(DateTime day, int index, string toDo)
    {
        var dayAdapted = day.ToString(CultureInfo.CurrentCulture).Split(' ')[0];
        if (IsSheetExists(day))
        {
            var strings = System.IO.File.ReadAllLines($"{FolderName}//{dayAdapted}.csv").ToList();
            strings.Add($"{index};{toDo}");
            System.IO.File.WriteAllLines($"{FolderName}//{dayAdapted}.csv", strings);
        }
        else
        {
            var stream = System.IO.File.Create($"{FolderName}//{dayAdapted}.csv");
            stream.Close();
            var strings = new List<string>();
            strings.Add("index;todo");
            strings.Add($"{index};{toDo}");
            System.IO.File.WriteAllLines($"{FolderName}//{dayAdapted}.csv", strings);
        }

        return RedirectToAction("GetAllActions");
    }

    private bool IsSheetExists(DateTime day)
    {
        var dayAdapted = day.ToString(CultureInfo.CurrentCulture).Split(' ')[0];
        var folder = new DirectoryInfo(FolderName);
        return folder.GetFiles().Select(file => file.Name.Equals($"{dayAdapted}.csv")).Any(flag => flag);
    }
}