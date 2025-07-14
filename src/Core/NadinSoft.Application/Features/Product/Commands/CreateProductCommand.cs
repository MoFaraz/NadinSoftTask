using FluentValidation;
using Mediator;
using NadinSoft.Application.Common;
using NadinSoft.Application.Common.Validation;

namespace NadinSoft.Application.Features.Product.Commands;

public record CreateProductCommand(
    string Name,
    string ManufacturePhone,
    string ManufactureEmail,
    DateTime ProduceDate,
    Guid UserId) : IRequest<OperationResult<bool>>, IValidatableModel<CreateProductCommand>
{
    public IValidator<CreateProductCommand> Validate(ValidationModelBase<CreateProductCommand> validator)
    {
        validator.RuleFor(c => c.Name).NotEmpty()
            .MinimumLength(3)
            .WithMessage("Name is required");

        validator.RuleFor(c => c.ManufacturePhone).NotEmpty().WithMessage("Phone number is required.")
            .Matches(@"^(?:\+98|98|0)?9\d{9}$")
            .WithMessage("Phone number is not valid.");
        
        validator.RuleFor(c => c.ProduceDate).NotEmpty()
            .WithMessage("Produce date is required");

        validator.RuleFor(c => c.ManufactureEmail).NotEmpty().EmailAddress()
            .WithMessage("Manufacture email is required");

        validator.RuleFor(c => c.UserId).NotEmpty()
            .WithMessage("User id is required");

        return validator;
    }
}