using FluentValidation;
using Mediator;
using NadinSoft.Application.Common;
using NadinSoft.Application.Common.Validation;

namespace NadinSoft.Application.Features.Product.Commands;

public record EditProductCommand(
    Guid Id,
    string Name,
    string ManufacturePhone,
    string ManufactureEmail,
    DateTime ProduceDate) : IRequest<OperationResult<bool>>, IValidatableModel<EditProductCommand>
{
    public IValidator<EditProductCommand> Validate(ValidationModelBase<EditProductCommand> validator)
    {
        validator.RuleFor(c => c.Name).NotEmpty().MinimumLength(3);
        validator.RuleFor(c => c.ManufacturePhone).NotEmpty(); // TODO Add Regex for phone number
        validator.RuleFor(c => c.ManufactureEmail).NotEmpty().EmailAddress();
        validator.RuleFor(c => c.ProduceDate).NotEmpty();

        return validator;
    }
}