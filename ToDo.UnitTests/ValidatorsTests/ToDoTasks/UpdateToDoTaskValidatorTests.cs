using FluentValidation.TestHelper;
using ToDo.BLL.Commands.ToDoTasks.Update;
using ToDo.BLL.Constants;
using ToDo.BLL.DTOs.ToDoTasks;
using ToDo.BLL.Validators.ToDoTasks;
using ToDo.DAL.Enums;

namespace ToDo.UnitTests.ValidatorsTests.ToDoTasks;

public class UpdateToDoTaskValidatorTests
{
    private readonly UpdateToDoTaskValidator _validator;

    public UpdateToDoTaskValidatorTests()
    {
        _validator = new UpdateToDoTaskValidator(new BaseToDoTaskValidator());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Validate_ShouldHaveError_WhenTitleIsNotValid(string? title)
    {
        var command = new UpdateToDoTaskCommand(new UpdateToDoTaskDto { Title = title }, 1);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.updateToDoTaskDto.Title)
            .WithErrorMessage(ErrorMessagesConstants.PropertyIsRequired(nameof(ToDoTaskDto.Title)));
    }

    [Fact]
    public void Validate_ShouldHaveError_WhenTitleExceedsMaximumLength()
    {
        var command = new UpdateToDoTaskCommand(new UpdateToDoTaskDto { Title = new string('a', 51) }, 1);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.updateToDoTaskDto.Title)
            .WithErrorMessage(ErrorMessagesConstants
                .PropertyMustHaveAMaximumLengthOfNCharacters(nameof(ToDoTaskDto.Title), ToDoTaskConstants.MaxTitleLength));
    }

    [Fact]
    public void Validate_ShouldNotHaveError_WhenTitleIsValid()
    {
        var command = new UpdateToDoTaskCommand(new UpdateToDoTaskDto { Title = "Valid Title" }, 1);

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveValidationErrorFor(c => c.updateToDoTaskDto.Title);
    }

    [Fact]
    public void Validate_ShouldNotHaveError_WhenDescriptionIsNull()
    {
        var command = new UpdateToDoTaskCommand(new UpdateToDoTaskDto { Description = null }, 1);

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveValidationErrorFor(c => c.updateToDoTaskDto.Description);
    }

    [Fact]
    public void Validate_ShouldHaveError_WhenDescriptionExceedsMaximumLength()
    {
        var command = new UpdateToDoTaskCommand(new UpdateToDoTaskDto { Description = new string('a', 251) }, 1);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.updateToDoTaskDto.Description)
            .WithErrorMessage(ErrorMessagesConstants
                .PropertyMustHaveAMaximumLengthOfNCharacters(nameof(ToDoTaskDto.Description), ToDoTaskConstants.MaxDescriptionLength));
    }

    [Fact]
    public void Validate_ShouldNotHaveError_WhenDescriptionIsValid()
    {
        var command = new UpdateToDoTaskCommand(new UpdateToDoTaskDto { Description = "Valid description." }, 1);

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveValidationErrorFor(c => c.updateToDoTaskDto.Description);
    }

    [Theory]
    [InlineData(null)]
    public void Validate_ShouldHaveError_WhenDeadlineIsInvalid(DateTime? deadline)
    {
        var command = new UpdateToDoTaskCommand(new UpdateToDoTaskDto { Deadline = deadline }, 1);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.updateToDoTaskDto.Deadline)
            .WithErrorMessage(ErrorMessagesConstants.PropertyIsRequired(nameof(ToDoTaskDto.Deadline)));
    }

    [Fact]
    public void Validate_ShouldNotHaveError_WhenDeadlineIsValid()
    {
        var command = new UpdateToDoTaskCommand(new UpdateToDoTaskDto { Deadline = new DateTime(2025, 12, 31) }, 1);

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveValidationErrorFor(c => c.updateToDoTaskDto.Deadline);
    }

    [Fact]
    public void Validate_ShouldNotHaveError_WhenStatusIsValid()
    {
        var command = new UpdateToDoTaskCommand(new UpdateToDoTaskDto { Status = Status.InProgress }, 1); // Assuming "Pending" is a valid enum value

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveValidationErrorFor(c => c.updateToDoTaskDto.Status);
    }
}
