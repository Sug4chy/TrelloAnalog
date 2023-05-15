using System.ComponentModel.DataAnnotations;

namespace Learning_ASP.NET.Models;

public class ToDoAction
{
    public int Index { get; init; }

    [Required(ErrorMessage = "Нельзя ничего не делать")]
    public string ToDo { get; init; } = null!;

    public override string ToString()
    {
        return $"{Index} - {ToDo}";
    }
}