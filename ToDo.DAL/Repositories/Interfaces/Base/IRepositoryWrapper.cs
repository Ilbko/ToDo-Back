using System.Transactions;
using ToDo.DAL.Repositories.Interfaces.ToDoTasks;

namespace ToDo.DAL.Repositories.Interfaces.Base;

public interface IRepositoryWrapper
{
    IToDoTasksRepository ToDoTasksRepository { get; }

    int SaveChanges();
    Task<int> SaveChangesAsync();
    TransactionScope BeginTransaction();
}
