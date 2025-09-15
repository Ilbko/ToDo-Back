using FluentValidation;
using ToDo.BLL.Commands.ToDoTasks.Create;

namespace ToDo.BLL.Validators.ToDoTasks;

public class CreateToDoTaskValidator : AbstractValidator<CreateToDoTaskCommand>
{
    public CreateToDoTaskValidator(BaseToDoTaskValidator baseToDoTaskValidator)
    {
        RuleFor(x => x.createToDoTaskDto).SetValidator(baseToDoTaskValidator);
    }
}
