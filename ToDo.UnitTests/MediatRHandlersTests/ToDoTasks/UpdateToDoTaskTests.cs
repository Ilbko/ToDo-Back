using AutoMapper;
using Azure.Core;
using FluentValidation;
using Moq;
using ToDo.BLL.Commands.ToDoTasks.Update;
using ToDo.BLL.Constants;
using ToDo.BLL.DTOs.ToDoTasks;
using ToDo.BLL.Validators.ToDoTasks;
using ToDo.DAL.Entities;
using ToDo.DAL.Enums;
using ToDo.DAL.Repositories.Interfaces.Base;
using ToDo.DAL.Repositories.Options;

namespace ToDo.UnitTests.MediatRHandlersTests.ToDoTasks;

public class UpdateToDoTaskHandlerTests
{
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IRepositoryWrapper> _mockRepositoryWrapper;
    private readonly IValidator<UpdateToDoTaskCommand> _validator;

    private readonly ToDoTask _testExistingToDoTask = new ToDoTask()
    {
        Id = 1,
        Title = "Test Task",
        Description = "Test Description",
        Status = Status.ToDo,
        Deadline = new DateTime(2025, 1, 1),
    };

    private readonly ToDoTask _testUpdatedToDoTask = new ToDoTask()
    {
        Id = 1,
        Title = "Updated Title",
        Description = "Updated Description",
        Status = Status.InProgress,
        Deadline = new DateTime(2025, 2, 1),
    };

    private ToDoTaskDto _testUpdatedToDoTaskDto = new ToDoTaskDto()
    {
        Id = 1,
        Title = "Updated Title",
        Description = "Updated Description",
        Status = Status.InProgress,
        Deadline = new DateTime(2025, 2, 1),
    };

    public UpdateToDoTaskHandlerTests()
    {
        _mockMapper = new Mock<IMapper>();
        _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
        _validator = new UpdateToDoTaskValidator(new BaseToDoTaskValidator());
    }

    [Theory]
    [InlineData("Updated Description")]
    [InlineData(null)]
    [InlineData("")]
    public async Task Handle_ShouldUpdateToDoTask(string? testDescription)
    {
        _testUpdatedToDoTask.Description = testDescription;
        _testUpdatedToDoTaskDto = _testUpdatedToDoTaskDto with { Description = testDescription };
        SetupDependencies(_testExistingToDoTask);

        var handler = new UpdateToDoTaskHandler(_mockMapper.Object, _mockRepositoryWrapper.Object, _validator);

        var result = await handler.Handle(
            new UpdateToDoTaskCommand(new UpdateToDoTaskDto
            {
                Title = "Updated Title",
                Description = testDescription,
                Status = Status.InProgress,
                Deadline = new DateTime(2025, 2, 1),
            }, _testExistingToDoTask.Id), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(_testUpdatedToDoTaskDto.Title, result.Value.Title);
        Assert.Equal(_testUpdatedToDoTaskDto.Description, result.Value.Description);
        Assert.Equal(_testUpdatedToDoTaskDto.Status, result.Value.Status);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task Handle_ShouldNotUpdateToDoTask_IncorrectTitle(string? testTitle)
    {
        _testUpdatedToDoTask.Title = testTitle!;
        _testUpdatedToDoTaskDto = _testUpdatedToDoTaskDto with { Title = testTitle! };
        SetupDependencies(_testExistingToDoTask);

        var handler = new UpdateToDoTaskHandler(_mockMapper.Object, _mockRepositoryWrapper.Object, _validator);

        var result = await handler.Handle(
            new UpdateToDoTaskCommand(new UpdateToDoTaskDto
            {
                Title = testTitle,
                Description = "Updated Description",
                Status = Status.InProgress,
                Deadline = new DateTime(2025, 2, 1),
            }, _testExistingToDoTask.Id), CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Contains("Validation failed", result.Errors[0].Message);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public async Task Handle_ShouldNotUpdateToDoTask_NotFound(long testId)
    {
        SetupDependencies(null, -1);
        var handler = new UpdateToDoTaskHandler(_mockMapper.Object, _mockRepositoryWrapper.Object, _validator);

        var result = await handler.Handle(
            new UpdateToDoTaskCommand(new UpdateToDoTaskDto
            {
                Title = "Updated Title",
                Description = "Updated Description",
                Status = Status.InProgress,
                Deadline = new DateTime(2025, 2, 1),
            }, testId), CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal(ToDoTaskConstants.FailedToUpdateToDoTask, result.Errors[0].Message);
    }

    [Fact]
    public async Task Handle_ShouldNotUpdateToDoTask_SaveChangesFails()
    {
        SetupDependencies(_testExistingToDoTask, -1);
        var handler = new UpdateToDoTaskHandler(_mockMapper.Object, _mockRepositoryWrapper.Object, _validator);

        var result = await handler.Handle(
            new UpdateToDoTaskCommand(new UpdateToDoTaskDto
            {
                Title = "Updated Title",
                Description = "Updated Description",
                Status = Status.InProgress,
                Deadline = new DateTime(2025, 2, 1),
            }, _testExistingToDoTask.Id), CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal(ToDoTaskConstants.FailedToUpdateToDoTask, result.Errors[0].Message);
    }

    private void SetupDependencies(ToDoTask? taskToReturn = null, int saveResult = 1)
    {
        SetupMapper();
        SetupRepositoryWrapper(taskToReturn, saveResult);
    }

    private void SetupMapper()
    {
        _mockMapper.Setup(x => x.Map<UpdateToDoTaskDto, ToDoTask>(It.IsAny<UpdateToDoTaskDto>()))
            .Returns(_testUpdatedToDoTask);

        _mockMapper.Setup(x => x.Map<ToDoTask, ToDoTaskDto>(It.IsAny<ToDoTask>()))
            .Returns(_testUpdatedToDoTaskDto);
    }

    private void SetupRepositoryWrapper(ToDoTask? taskToReturn = null, int saveResult = 1)
    {
        _mockRepositoryWrapper.Setup(x => x.ToDoTasksRepository.GetFirstOrDefaultAsync(
                It.IsAny<QueryOptions<ToDoTask>>()))
            .ReturnsAsync(taskToReturn);

        _mockRepositoryWrapper.Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(saveResult);
    }
}
