using ToDo.DAL.Enums;

namespace ToDo.BLL.DTOs.ToDoTasks;

public record ToDoTaskDto
{
    public long Id { get; init; }
    public string Title { get; init; } = null!;
    public string? Description { get; init; }
    public DateTime? Deadline { get; init; }
    public Status Status { get; init; }
}
