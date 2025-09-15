using FluentResults;
using MediatR;
using ToDo.BLL.DTOs.ToDoTasks;

namespace ToDo.BLL.Commands.ToDoTasks.Update;

public record UpdateToDoTaskCommand(UpdateToDoTaskDto updateToDoTaskDto, long id)
    : IRequest<Result<ToDoTaskDto>>;
