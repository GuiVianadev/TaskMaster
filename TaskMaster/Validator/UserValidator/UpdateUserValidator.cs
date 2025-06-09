using FluentValidation;
using TaskMaster.Dtos.UserDTOs;

namespace TaskMaster.Validator.UserValidator;

public class UpdateUserValidator : AbstractValidator<UpdateUserDTO>
{
    public UpdateUserValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name cannot be empty");
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email cannot be empty")
            .EmailAddress().WithMessage("Please enter a valid email address");
        RuleFor(user => user.Password)
            .NotEmpty().WithMessage("Password cannot be empty")
            .MinimumLength(6).WithMessage("Password must have at least 6 characters");
    }
}