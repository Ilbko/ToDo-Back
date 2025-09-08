using ToDo.DAL.Enums;

namespace ToDo.DAL.Entities;

public class ToDoTask
{
    public long Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public DateTime Deadline { get; set; }
    public long UserId { get; set; }

    public Status Status { get; set; }
    public User User { get; set; }
}