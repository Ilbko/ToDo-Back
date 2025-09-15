namespace ToDo.BLL.DTOs.ToDoTasks;

public record CreateToDoTaskDto
{
    public string Title { get; init; } = null!;
    public string? Description { get; init; }
    public DateTime Deadline { get; init; }
}