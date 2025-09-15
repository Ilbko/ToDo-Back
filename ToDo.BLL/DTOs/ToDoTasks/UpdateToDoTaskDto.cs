using ToDo.DAL.Enums;

namespace ToDo.BLL.DTOs.ToDoTasks;

public record UpdateToDoTaskDto: CreateToDoTaskDto
{
    public Status? Status { get; init; }
}