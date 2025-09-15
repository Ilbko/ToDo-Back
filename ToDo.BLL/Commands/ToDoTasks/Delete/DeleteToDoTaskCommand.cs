using FluentResults;
using MediatR;

namespace ToDo.BLL.Commands.ToDoTasks.Delete;

public record DeleteToDoTaskCommand(long id) 
    : IRequest<Result<long>>;
