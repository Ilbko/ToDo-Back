using FluentResults;
using MediatR;
using ToDo.BLL.Constants;
using ToDo.DAL.Entities;
using ToDo.DAL.Repositories.Interfaces.Base;
using ToDo.DAL.Repositories.Options;

namespace ToDo.BLL.Commands.ToDoTasks.Delete;

public class DeleteToDoTaskHandler : IRequestHandler<DeleteToDoTaskCommand, Result<long>>
{
    private readonly IRepositoryWrapper _repositoryWrapper;

    public DeleteToDoTaskHandler(IRepositoryWrapper repositoryWrapper)
    {
        _repositoryWrapper = repositoryWrapper;
    }

    public async Task<Result<long>> Handle(DeleteToDoTaskCommand request, CancellationToken cancellationToken)
    {
        var entityToDelete = 
            await _repositoryWrapper.ToDoTasksRepository.GetFirstOrDefaultAsync(new QueryOptions<ToDoTask>
            {
                Filter = entity => entity.Id == request.id,
            });

        if (entityToDelete is null)
        {
            return Result.Fail<long>(ErrorMessagesConstants.NotFound(request.id, typeof(ToDoTask)));
        }

        _repositoryWrapper.ToDoTasksRepository.Delete(entityToDelete);

        if (await _repositoryWrapper.SaveChangesAsync() > 0)
        {
            return Result.Ok(entityToDelete.Id);
        }

        return Result.Fail<long>(ToDoTaskConstants.FailedToDeleteToDoTask);
    }
}

