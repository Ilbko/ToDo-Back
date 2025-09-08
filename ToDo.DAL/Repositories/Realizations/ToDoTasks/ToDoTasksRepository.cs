using ToDo.DAL.Data;
using ToDo.DAL.Entities;
using ToDo.DAL.Repositories.Interfaces.ToDoTasks;
using ToDo.DAL.Repositories.Realizations.Base;

namespace ToDo.DAL.Repositories.Realizations.ToDoTasks;

internal class ToDoTasksRepository : RepositoryBase<ToDoTask>, IToDoTasksRepository
{
    public ToDoTasksRepository(ToDoDbContext context)
        : base(context) { }
}
