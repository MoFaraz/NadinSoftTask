using FluentValidation;
using Mediator;
using NadinSoft.Application.Common;
using NadinSoft.Application.Common.Validation;

namespace NadinSoft.Application.Features.Product.Commands;

public record ChangeProductAvailabilityCommand(Guid Id, bool IsAvailable, Guid UserId)
    : IRequest<OperationResult<bool>>, IValidatableModel<ChangeProductAvailabilityCommand>
{
    public IValidator<ChangeProductAvailabilityCommand> Validate(
        ValidationModelBase<ChangeProductAvailabilityCommand> validator)
    {
        validator.RuleFor(c => c.Id).NotEmpty();
        validator.RuleFor(c => c.UserId).NotEmpty();
        validator.RuleFor(c => c.IsAvailable).NotNull();
        return validator;
    }
}