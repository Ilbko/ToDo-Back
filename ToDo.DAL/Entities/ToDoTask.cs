using ToDo.DAL.Enums;

namespace ToDo.DAL.Entities;

public class ToDoTask
{
    public long Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public DateTime Deadline { get; set; }

    public Status Status { get; set; }
}