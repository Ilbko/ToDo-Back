using FluentValidation;
using ToDo.BLL.Commands.ToDoTasks.Update;
using ToDo.BLL.Constants;
using ToDo.BLL.DTOs.ToDoTasks;

namespace ToDo.BLL.Validators.ToDoTasks;

public class UpdateToDoTaskValidator : AbstractValidator<UpdateToDoTaskCommand>
{
    public UpdateToDoTaskValidator(BaseToDoTaskValidator baseToDoTaskValidator)
    {
        RuleFor(x => x.updateToDoTaskDto).SetValidator(baseToDoTaskValidator);

        RuleFor(x => x.updateToDoTaskDto.Status)
            .IsInEnum()
            .WithMessage(ErrorMessagesConstants.PropertyMustBeValidEnum(nameof(ToDoTaskDto.Status)));
    }
}