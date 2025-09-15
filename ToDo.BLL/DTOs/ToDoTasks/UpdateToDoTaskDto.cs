using ToDo.DAL.Enums;

namespace ToDo.BLL.DTOs.ToDoTasks;

public record UpdateToDoTaskDto
{
    public string? Title { get; init; }
    public string? Description { get; init; }
    public DateTime? Deadline { get; init; }
    public Status? Status { get; init; }
}