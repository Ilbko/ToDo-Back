using System.Transactions;
using ToDo.DAL.Data;
using ToDo.DAL.Repositories.Interfaces.Base;
using ToDo.DAL.Repositories.Interfaces.ToDoTasks;
using ToDo.DAL.Repositories.Realizations.ToDoTasks;

namespace ToDo.DAL.Repositories.Realizations.Base;

internal class RepositoryWrapper : IRepositoryWrapper
{
    private readonly ToDoDbContext _dbContext;

    private IToDoTasksRepository? _toDoTasksRepository;

    public RepositoryWrapper(ToDoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IToDoTasksRepository ToDoTasksRepository => _toDoTasksRepository ?? new ToDoTasksRepository(_dbContext);

    public TransactionScope BeginTransaction()
    {
        return new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
    }

    public int SaveChanges()
    {
        return _dbContext.SaveChanges();
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _dbContext.SaveChangesAsync();
    }
}
