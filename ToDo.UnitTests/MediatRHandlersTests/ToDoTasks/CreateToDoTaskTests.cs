using AutoMapper;
using FluentValidation;
using Moq;
using ToDo.BLL.Commands.ToDoTasks.Create;
using ToDo.BLL.Constants;
using ToDo.BLL.DTOs.ToDoTasks;
using ToDo.BLL.Validators.ToDoTasks;
using ToDo.DAL.Entities;
using ToDo.DAL.Enums;
using ToDo.DAL.Repositories.Interfaces.Base;

namespace ToDo.UnitTests.MediatRHandlersTests.ToDoTasks;

public class CreateToDoTaskTests
{
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IRepositoryWrapper> _repositoryWrapperMock;
    private readonly IValidator<CreateToDoTaskCommand> _validator;

    private readonly ToDoTask _testEntity = new()
    {
        Id = 1,
        Title = "Test Task",
        Description = "Test Task Description",
        Deadline = new DateTime(2025, 1, 1),
    };

    private ToDoTaskDto _testToDoTaskDto = new()
    {
        Title = "Test Task",
        Description = "Test Task Description",
        Deadline = new DateTime(2025, 1, 1),
    };

    public CreateToDoTaskTests()
    {
        _mapperMock = new Mock<IMapper>();
        _repositoryWrapperMock = new Mock<IRepositoryWrapper>();
        _validator = new CreateToDoTaskValidator(new BaseToDoTaskValidator());
    }

    [Theory]
    [InlineData("Test Task", "Test Task Description", Status.ToDo, "2025-01-01")]
    public async Task Handle_ShouldCreateToDoTask(string title, string? description, Status status, string deadline)
    {
        _testEntity.Title = title;
        _testEntity.Description = description;
        _testEntity.Status = status;
        _testEntity.Deadline = DateTime.Parse(deadline);

        _testToDoTaskDto = _testToDoTaskDto with
        {
            Title = title,
            Description = description,
            Deadline = DateTime.Parse(deadline)
        };

        SetupDependencies();
        var handler = new CreateToDoTaskHandler(_mapperMock.Object, _repositoryWrapperMock.Object, _validator);

        var result = await handler.Handle(
            new CreateToDoTaskCommand(new CreateToDoTaskDto
            {
                Title = title,
                Description = description,
                Deadline = DateTime.Parse(deadline)
            }), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(result.Value.Title, _testToDoTaskDto.Title);
        Assert.Equal(result.Value.Description, _testToDoTaskDto.Description);
        Assert.Equal(result.Value.Status, _testToDoTaskDto.Status);
        Assert.Equal(result.Value.Deadline, _testToDoTaskDto.Deadline);
    }

    [Theory]
    [InlineData(" ", "Test Task Description", Status.ToDo, "2025-01-01")]
    [InlineData("", null, Status.ToDo, "2025-01-01")]
    public async Task Handle_ShouldFail_InvalidTitle(string title, string? description, Status status, string deadline)
    {
        _testEntity.Title = title;
        _testEntity.Description = description;
        _testEntity.Status = status;
        _testEntity.Deadline = DateTime.Parse(deadline);

        _testToDoTaskDto = _testToDoTaskDto with
        {
            Title = title,
            Description = description,
            Status = status,
            Deadline = DateTime.Parse(deadline)
        };

        SetupDependencies();
        var handler = new CreateToDoTaskHandler(_mapperMock.Object, _repositoryWrapperMock.Object, _validator);

        var result = await handler.Handle(
            new CreateToDoTaskCommand(new CreateToDoTaskDto
            {
                Title = title,
                Description = description,
                Deadline = DateTime.Parse(deadline)
            }), CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Contains("Validation failed", result.Errors[0].Message);
    }

    [Fact]
    public async Task Handle_ShouldFail_SaveChangesFails()
    {
        SetupDependencies(-1);
        var handler = new CreateToDoTaskHandler(_mapperMock.Object, _repositoryWrapperMock.Object, _validator);

        var result = await handler.Handle(
            new CreateToDoTaskCommand(new CreateToDoTaskDto
            {
                Title = "Test Task",
                Description = "Test Task Description",
                Deadline = new DateTime(2025, 1, 1)
            }), CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal(ToDoTaskConstants.FailedToCreateToDoTask, result.Errors[0].Message);
    }

    private void SetupDependencies(int saveResult = 1)
    {
        SetupMapper(_testEntity, _testToDoTaskDto);
        SetupRepositoryWrapper(saveResult);
    }

    private void SetupMapper(ToDoTask outputToDoTaskEntity, ToDoTaskDto outputToDoTaskDto)
    {
        _mapperMock.Setup(m => m.Map<ToDoTask>(It.IsAny<CreateToDoTaskDto>()))
            .Returns(outputToDoTaskEntity);
        _mapperMock.Setup(m => m.Map<ToDoTaskDto>(It.IsAny<ToDoTask>()))
            .Returns(outputToDoTaskDto);
    }

    private void SetupRepositoryWrapper(int saveResult)
    {
        _repositoryWrapperMock.Setup(repo => repo.ToDoTasksRepository.CreateAsync(It.IsAny<ToDoTask>()));
        _repositoryWrapperMock.Setup(repo => repo.SaveChangesAsync()).ReturnsAsync(saveResult);
    }
}
