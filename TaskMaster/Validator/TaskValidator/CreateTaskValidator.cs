using FluentValidation;
using TaskMaster.Dtos.TaskDTOs;

namespace TaskMaster.Validator.TaskValidator;

public class CreateTaskValidator : AbstractValidator<CreateTaskDTO>
{
    public CreateTaskValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title cannot be empty")
            .MaximumLength(100).WithMessage("Title cannot be longer than 100 characters");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description cannot be empty");
        RuleFor(x => x.Priority)
            .IsInEnum()
            .WithMessage("Priority invalid");
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId cannot be empty");
    }
}
