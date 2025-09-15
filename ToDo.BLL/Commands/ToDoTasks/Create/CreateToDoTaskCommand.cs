using FluentResults;
using MediatR;
using ToDo.BLL.DTOs.ToDoTasks;

namespace ToDo.BLL.Commands.ToDoTasks.Create;

public record CreateToDoTaskCommand(CreateToDoTaskDto createToDoTaskDto)
    : IRequest<Result<ToDoTaskDto>>;
