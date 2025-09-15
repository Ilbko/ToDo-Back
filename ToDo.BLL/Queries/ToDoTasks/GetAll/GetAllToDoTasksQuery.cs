using FluentResults;
using MediatR;
using ToDo.BLL.DTOs.ToDoTasks;

namespace ToDo.BLL.Queries.ToDoTasks.GetAll;

public record GetAllToDoTasksQuery() 
    : IRequest<Result<List<ToDoTaskDto>>>;