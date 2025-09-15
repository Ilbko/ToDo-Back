using AutoMapper;
using FluentResults;
using FluentValidation;
using MediatR;
using ToDo.BLL.DTOs.ToDoTasks;
using ToDo.DAL.Entities;
using ToDo.DAL.Repositories.Interfaces.Base;

namespace ToDo.BLL.Commands.ToDoTasks.Create;

public class CreateToDoTaskHandler : IRequestHandler<CreateToDoTaskCommand, Result<ToDoTaskDto>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IValidator<CreateToDoTaskCommand> _validator;

    public CreateToDoTaskHandler(
        IMapper mapper,
        IRepositoryWrapper repositoryWrapper,
        IValidator<CreateToDoTaskCommand> validator)
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
        _validator = validator;
    }

    public async Task<Result<ToDoTaskDto>> Handle(CreateToDoTaskCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _validator.ValidateAndThrowAsync(request, cancellationToken);

            var entity = _mapper.Map<ToDoTask>(request.createToDoTaskDto);

            await _repositoryWrapper.ToDoTasksRepository.CreateAsync(entity);

            if (await _repositoryWrapper.SaveChangesAsync() > 0)
            {
                var resultDto = _mapper.Map<ToDoTaskDto>(entity);
                return Result.Ok(resultDto);
            }

            return Result.Fail<ToDoTaskDto>("Failed to create a task.");
        }
        catch (ValidationException ex)
        {
            return Result.Fail<ToDoTaskDto>(ex.Message);
        }
    }
}
