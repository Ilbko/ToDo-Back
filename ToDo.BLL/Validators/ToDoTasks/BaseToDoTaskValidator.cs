using FluentValidation;
using ToDo.BLL.Constants;
using ToDo.BLL.DTOs.ToDoTasks;

namespace ToDo.BLL.Validators.ToDoTasks;

public class BaseToDoTaskValidator: AbstractValidator<CreateToDoTaskDto>
{
    public BaseToDoTaskValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage(ErrorMessagesConstants.PropertyIsRequired(nameof(ToDoTaskDto.Title)))
            .MaximumLength(ToDoTaskConstants.MaxTitleLength)
            .WithMessage(ErrorMessagesConstants
                .PropertyMustHaveAMaximumLengthOfNCharacters(nameof(ToDoTaskDto.Title), ToDoTaskConstants.MaxTitleLength));

        RuleFor(x => x.Description)
            .MaximumLength(ToDoTaskConstants.MaxDescriptionLength)
            .WithMessage(ErrorMessagesConstants
                .PropertyMustHaveAMaximumLengthOfNCharacters(nameof(ToDoTaskDto.Description), ToDoTaskConstants.MaxDescriptionLength));

        RuleFor(x => x.Deadline)
            .NotEmpty()
            .WithMessage(ErrorMessagesConstants.PropertyIsRequired(nameof(ToDoTaskDto.Deadline)));
    }
}
