using Microsoft.AspNetCore.Mvc;
using ToDo.BLL.Commands.ToDoTasks.Create;
using ToDo.BLL.Commands.ToDoTasks.Delete;
using ToDo.BLL.Commands.ToDoTasks.Update;
using ToDo.BLL.DTOs.ToDoTasks;
using ToDo.BLL.Queries.ToDoTasks.GetAll;

namespace ToDo.WebAPI.Controllers.ToDoTasks;

public class ToDoTasksController : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetToDoTasks()
    {
        return HandleResult(await Mediator.Send(new GetAllToDoTasksQuery()));
    }

    [HttpPost]
    public async Task<IActionResult> CreateToDoTask([FromBody] CreateToDoTaskDto createToDoTaskDto)
    {
        return HandleResult(await Mediator.Send(new CreateToDoTaskCommand(createToDoTaskDto)));
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> UpdateToDoTask([FromBody] UpdateToDoTaskDto updateToDoTaskDto, long id)
    {
        return HandleResult(await Mediator.Send(new UpdateToDoTaskCommand(updateToDoTaskDto, id)));
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteToDoTask(long id)
    {
        return HandleResult(await Mediator.Send(new DeleteToDoTaskCommand(id)));
    }
}
