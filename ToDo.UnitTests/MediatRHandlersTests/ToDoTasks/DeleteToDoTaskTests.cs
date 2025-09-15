using Moq;
using ToDo.BLL.Commands.ToDoTasks.Delete;
using ToDo.BLL.Constants;
using ToDo.DAL.Entities;
using ToDo.DAL.Repositories.Interfaces.Base;
using ToDo.DAL.Repositories.Options;

namespace ToDo.UnitTests.MediatRHandlersTests.ToDoTasks;

public class DeleteToDoTaskTests
{
    private readonly Mock<IRepositoryWrapper> _mockRepositoryWrapper;

    private readonly ToDoTask _testExistingToDoTask = new ToDoTask()
    {
        Id = 1,
        Title = "Test Task",
        Description = "Test Task Description",
        Status = ToDo.DAL.Enums.Status.ToDo,
        Deadline = new DateTime(2025, 1, 1),
    };

    public DeleteToDoTaskTests()
    {
        _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
    }

    [Fact]
    public async Task Handle_ShouldDeleteToDoTask()
    {
        SetupRepositoryWrapper(_testExistingToDoTask);
        var handler = new DeleteToDoTaskHandler(_mockRepositoryWrapper.Object);

        var result = await handler.Handle(new DeleteToDoTaskCommand(_testExistingToDoTask.Id), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(_testExistingToDoTask.Id, result.Value);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public async Task Handle_ShouldNotDeleteToDoTask_TaskNotFound(long taskId)
    {
        SetupRepositoryWrapper(null);
        var handler = new DeleteToDoTaskHandler(_mockRepositoryWrapper.Object);

        var result = await handler.Handle(new DeleteToDoTaskCommand(taskId), CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorMessagesConstants.NotFound(taskId, typeof(ToDoTask)), result.Errors[0].Message);
    }

    [Fact]
    public async Task Handle_ShouldNotDeleteToDoTask_SaveChangesFails()
    {
        SetupRepositoryWrapper(_testExistingToDoTask, -1);
        var handler = new DeleteToDoTaskHandler(_mockRepositoryWrapper.Object);

        var result = await handler.Handle(new DeleteToDoTaskCommand(_testExistingToDoTask.Id), CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal(ToDoTaskConstants.FailedToDeleteToDoTask, result.Errors[0].Message);
    }

    private void SetupRepositoryWrapper(ToDoTask? entityToDelete = null, int saveResult = 1)
    {
        _mockRepositoryWrapper.Setup(x => x.ToDoTasksRepository.GetFirstOrDefaultAsync(
                It.IsAny<QueryOptions<ToDoTask>>()))
            .ReturnsAsync(entityToDelete);

        _mockRepositoryWrapper.Setup(x => x.SaveChangesAsync()).ReturnsAsync(saveResult);
    }
}
