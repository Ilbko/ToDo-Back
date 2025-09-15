using AutoMapper;
using Moq;
using ToDo.BLL.DTOs.ToDoTasks;
using ToDo.BLL.Queries.ToDoTasks.GetAll;
using ToDo.DAL.Entities;
using ToDo.DAL.Enums;
using ToDo.DAL.Repositories.Interfaces.Base;
using ToDo.DAL.Repositories.Options;

namespace ToDo.UnitTests.MediatRHandlersTests.ToDoTasks;

public class GetAllToDoTasksTests
{
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IRepositoryWrapper> _mockRepositoryWrapper;

    private readonly List<ToDoTask> _testToDoTasks = new List<ToDoTask>
        {
            new ToDoTask
            {
                Id = 1,
                Title = "Task 1",
                Description = "Test description 1",
                Deadline = new DateTime(2025, 1, 1),
                Status = Status.ToDo
            },
            new ToDoTask
            {
                Id = 2,
                Title = "Task 2",
                Description = "Test description 2",
                Deadline = new DateTime(2025, 2, 1),
                Status = Status.InProgress
            }
        };

    private readonly List<ToDoTaskDto> _testToDoTaskDtos = new List<ToDoTaskDto>
        {
            new ToDoTaskDto
            {
                Id = 1,
                Title = "Task 1",
                Description = "Test description 1",
                Deadline = new DateTime(2025, 1, 1),
                Status = Status.ToDo
            },
            new ToDoTaskDto
            {
                Id = 2,
                Title = "Task 2",
                Description = "Test description 2",
                Deadline = new DateTime(2025, 2, 1),
                Status = Status.InProgress
            }
        };

    public GetAllToDoTasksTests()
    {
        _mockMapper = new Mock<IMapper>();
        _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
    }

    [Fact]
    public async Task Handle_ShouldReturnAllToDoTasks()
    {
        SetupDependencies(_testToDoTasks);

        var handler = new GetAllToDoTasksHandler(_mockMapper.Object, _mockRepositoryWrapper.Object);

        var result = await handler.Handle(new GetAllToDoTasksQuery(), CancellationToken.None);

        Assert.NotNull(result);
        Assert.NotEmpty(result.Value);
        Assert.Equal(2, result.Value.Count);
        Assert.Equal(_testToDoTaskDtos[0].Title, result.Value[0].Title);
    }

    private void SetupDependencies(List<ToDoTask> tasksToReturn)
    {
        _mockRepositoryWrapper.Setup(repo => repo.ToDoTasksRepository.GetAllAsync(It.IsAny<QueryOptions<ToDoTask>>()))
            .ReturnsAsync(tasksToReturn);

        _mockMapper.Setup(x => x.Map<IEnumerable<ToDoTaskDto>>(It.IsAny<IEnumerable<ToDoTask>>()))
            .Returns(_testToDoTaskDtos);
    }
}
