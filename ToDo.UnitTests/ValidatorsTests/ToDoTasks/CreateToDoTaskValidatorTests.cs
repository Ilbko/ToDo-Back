using FluentValidation.TestHelper;
using ToDo.BLL.Commands.ToDoTasks.Create;
using ToDo.BLL.Constants;
using ToDo.BLL.DTOs.ToDoTasks;
using ToDo.BLL.Validators.ToDoTasks;

namespace ToDo.UnitTests.ValidatorsTests.ToDoTasks;

public class CreateToDoTaskValidatorTests
{
    private readonly CreateToDoTaskValidator _validator;

    public CreateToDoTaskValidatorTests()
    {
        _validator = new CreateToDoTaskValidator(new BaseToDoTaskValidator());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Validate_ShouldHaveError_WhenTitleIsNotValid(string? title)
    {
        var command = new CreateToDoTaskCommand(new CreateToDoTaskDto { Title = title });

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.createToDoTaskDto.Title)
            .WithErrorMessage(ErrorMessagesConstants.PropertyIsRequired(nameof(ToDoTaskDto.Title)));
    }

    [Fact]
    public void Validate_ShouldHaveError_WhenTitleExceedsMaximumLength()
    {
        var command = new CreateToDoTaskCommand(new CreateToDoTaskDto { Title = new string('a', 51) });

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.createToDoTaskDto.Title)
            .WithErrorMessage(ErrorMessagesConstants
                .PropertyMustHaveAMaximumLengthOfNCharacters(nameof(ToDoTaskDto.Title), ToDoTaskConstants.MaxTitleLength));
    }

    [Fact]
    public void Validate_ShouldNotHaveError_WhenTitleIsValid()
    {
        var command = new CreateToDoTaskCommand(new CreateToDoTaskDto { Title = "Valid Title" });

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveValidationErrorFor(c => c.createToDoTaskDto.Title);
    }

    [Fact]
    public void Validate_ShouldNotHaveError_WhenDescriptionIsNull()
    {
        var command = new CreateToDoTaskCommand(new CreateToDoTaskDto { Description = null });

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveValidationErrorFor(c => c.createToDoTaskDto.Description);
    }

    [Fact]
    public void Validate_ShouldHaveError_WhenDescriptionExceedsMaximumLength()
    {
        var command = new CreateToDoTaskCommand(new CreateToDoTaskDto { Description = new string('a', 251) });

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.createToDoTaskDto.Description)
            .WithErrorMessage(ErrorMessagesConstants
                .PropertyMustHaveAMaximumLengthOfNCharacters(nameof(ToDoTaskDto.Description), ToDoTaskConstants.MaxDescriptionLength));
    }

    [Fact]
    public void Validate_ShouldNotHaveError_WhenDescriptionIsValid()
    {
        var command = new CreateToDoTaskCommand(new CreateToDoTaskDto { Description = "Valid description." });

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveValidationErrorFor(c => c.createToDoTaskDto.Description);
    }

    [Theory]
    [InlineData(null)]
    public void Validate_ShouldHaveError_WhenDeadlineIsNotValid(DateTime? deadline)
    {
        var command = new CreateToDoTaskCommand(new CreateToDoTaskDto { Deadline = deadline });

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.createToDoTaskDto.Deadline)
            .WithErrorMessage(ErrorMessagesConstants.PropertyIsRequired(nameof(ToDoTaskDto.Deadline)));
    }

    [Fact]
    public void Validate_ShouldNotHaveError_WhenDeadlineIsValid()
    {
        var command = new CreateToDoTaskCommand(new CreateToDoTaskDto { Deadline = new DateTime(2025, 12, 31) });

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveValidationErrorFor(c => c.createToDoTaskDto.Deadline);
    }
}