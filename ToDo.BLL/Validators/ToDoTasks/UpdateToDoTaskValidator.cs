using FluentValidation;
using ToDo.BLL.Commands.ToDoTasks.Update;

namespace ToDo.BLL.Validators.ToDoTasks;

public class UpdateToDoTaskValidator : AbstractValidator<UpdateToDoTaskCommand>
{
    public UpdateToDoTaskValidator()
    {
        RuleFor(x => x.updateToDoTaskDto.Title)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.updateToDoTaskDto.Description)
            .MaximumLength(250);

        RuleFor(x => x.updateToDoTaskDto.Deadline)
            .NotEmpty();

        RuleFor(x => x.updateToDoTaskDto.Status)
            .IsInEnum();
    }
}