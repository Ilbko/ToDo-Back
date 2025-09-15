using AutoMapper;
using ToDo.BLL.DTOs.ToDoTasks;
using ToDo.DAL.Entities;

namespace ToDo.BLL.Mapping.ToDoTasks;

internal class ToDoTasksProfile : Profile
{
    public ToDoTasksProfile()
    {
        CreateMap<CreateToDoTaskDto, ToDoTask>();
        CreateMap<UpdateToDoTaskDto, ToDoTask>();
        CreateMap<ToDoTask, ToDoTaskDto>();
    }
}
