namespace Learning_ASP.NET.Models;

public class ToDoActionsSheet
{
    public DateTime Date { get; init; }
    public List<ToDoAction> Actions { get; init; }
    
    public ToDoActionsSheet(string date, List<ToDoAction> actions)
    {
        Date = DateTime.Parse(date);
        Actions = actions;
    }

}