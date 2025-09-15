using AutoMapper;
using FluentResults;
using MediatR;
using ToDo.BLL.DTOs.ToDoTasks;
using ToDo.DAL.Repositories.Interfaces.Base;

namespace ToDo.BLL.Queries.ToDoTasks.GetAll;

public class GetAllToDoTasksHandler : IRequestHandler<GetAllToDoTasksQuery, Result<List<ToDoTaskDto>>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public GetAllToDoTasksHandler(IMapper mapper, IRepositoryWrapper repositoryWrapper)
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
    }

    public async Task<Result<List<ToDoTaskDto>>> Handle(GetAllToDoTasksQuery request, CancellationToken cancellationToken)
    {
        var entities = await _repositoryWrapper.ToDoTasksRepository.GetAllAsync();
        var mapped = _mapper.Map<IEnumerable<ToDoTaskDto>>(entities).ToList();
        
        return Result.Ok(mapped);
    }
}
