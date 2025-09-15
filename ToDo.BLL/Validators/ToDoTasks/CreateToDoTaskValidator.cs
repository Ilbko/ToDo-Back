using FluentValidation;
using ToDo.BLL.Commands.ToDoTasks.Create;

namespace ToDo.BLL.Validators.ToDoTasks;

public class CreateToDoTaskValidator : AbstractValidator<CreateToDoTaskCommand>
{
    public CreateToDoTaskValidator()
    {
        RuleFor(x => x.createToDoTaskDto.Title)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.createToDoTaskDto.Description)
            .MaximumLength(250);

        RuleFor(x => x.createToDoTaskDto.Deadline)
            .NotEmpty();
    }
}
