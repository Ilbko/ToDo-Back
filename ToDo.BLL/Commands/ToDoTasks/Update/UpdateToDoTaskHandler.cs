using AutoMapper;
using FluentResults;
using FluentValidation;
using MediatR;
using ToDo.BLL.Constants;
using ToDo.BLL.DTOs.ToDoTasks;
using ToDo.DAL.Entities;
using ToDo.DAL.Repositories.Interfaces.Base;
using ToDo.DAL.Repositories.Options;

namespace ToDo.BLL.Commands.ToDoTasks.Update;

public class UpdateToDoTaskHandler : IRequestHandler<UpdateToDoTaskCommand, Result<ToDoTaskDto>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IValidator<UpdateToDoTaskCommand> _validator;

    public UpdateToDoTaskHandler(
        IMapper mapper,
        IRepositoryWrapper repositoryWrapper,
        IValidator<UpdateToDoTaskCommand> validator)
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
        _validator = validator;
    }

    public async Task<Result<ToDoTaskDto>> Handle(UpdateToDoTaskCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _validator.ValidateAndThrowAsync(request, cancellationToken);

            var categoryEntity =
                await _repositoryWrapper.ToDoTasksRepository.GetFirstOrDefaultAsync(new QueryOptions<ToDoTask>
                {
                    Filter = entity => entity.Id == request.id,
                });

            var entityToUpdate = _mapper.Map<UpdateToDoTaskDto, ToDoTask>(request.updateToDoTaskDto);
            entityToUpdate.Id = request.id;

            _repositoryWrapper.ToDoTasksRepository.Update(entityToUpdate);

            if (await _repositoryWrapper.SaveChangesAsync() > 0)
            {
                var resultDto = _mapper.Map<ToDoTask, ToDoTaskDto>(entityToUpdate);
                return Result.Ok(resultDto);
            }

            return Result.Fail<ToDoTaskDto>(ToDoTaskConstants.FailedToUpdateToDoTask);
        }
        catch (ValidationException ex)
        {
            return Result.Fail<ToDoTaskDto>(ex.Message);
        }
    }
}
