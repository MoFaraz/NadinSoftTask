using FluentValidation;
using FluentValidation.Results;
using Mediator;
using NadinSoft.Application.Common;
using NadinSoft.Application.Common.Validation;

namespace NadinSoft.Application.Features.User.Commands.Register;

public record RegisterUserCommand(
    string FirstName,
    string LastName,
    string Username,
    string Password,
    string Email,
    string RepeatPassword) : IRequest<OperationResult<bool>>, IValidatableModel<RegisterUserCommand>
{
    public IValidator<RegisterUserCommand> Validate(ValidationModelBase<RegisterUserCommand> validator)
    {
        validator.RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Email is required.");
        validator.RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required.");
        validator.RuleFor(c => c.RepeatPassword).NotEmpty().Equal(c => c.Password).WithMessage("Passwords do not match.");
        validator.RuleFor(c => c.FirstName).NotEmpty().WithMessage("First name is required.");
        validator.RuleFor(c => c.LastName).NotEmpty().WithMessage("Last name is required.");
        validator.RuleFor(c => c.Username).NotEmpty().MinimumLength(3).MaximumLength(15)
            .WithMessage("Username must be between 3 and 15 characters.");
        return validator;
    }
}