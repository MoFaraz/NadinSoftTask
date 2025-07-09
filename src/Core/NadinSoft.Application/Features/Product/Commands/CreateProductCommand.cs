using FluentValidation;
using Mediator;
using NadinSoft.Application.Common;
using NadinSoft.Application.Common.Validation;

namespace NadinSoft.Application.Features.Product.Commands;

public record CreateProductCommand(
    Guid Id,
    string Name,
    string ManufacturePhone,
    string ManufactureEmail,
    DateTime ProduceDate,
    Guid UserId) : IRequest<OperationResult<bool>>, IValidatableModel<CreateProductCommand>
{
    public IValidator<CreateProductCommand> Validate(ValidationModelBase<CreateProductCommand> validator)
    {
        validator.RuleFor(c => c.Id).NotEmpty();
        validator.RuleFor(c => c.Name).NotEmpty()
            .MinimumLength(4)
            .WithMessage("Name is required");

        validator.RuleFor(c => c.ManufacturePhone).NotEmpty()
            .WithMessage("Manufacture phone is required"); // TODO add a regex here for phone number
        
        validator.RuleFor(c => c.ProduceDate).NotEmpty()
            .WithMessage("Produce date is required");
        
        validator.RuleFor(c => c.ManufactureEmail).NotEmpty().EmailAddress()
            .WithMessage("Manufacture email is required");
        
        validator.RuleFor(c => c.UserId).NotEmpty()
            .WithMessage("User id is required");

        return validator;
    }
}