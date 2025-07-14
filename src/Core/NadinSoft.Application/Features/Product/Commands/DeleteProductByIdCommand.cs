using FluentValidation;
using Mediator;
using NadinSoft.Application.Common;
using NadinSoft.Application.Common.Validation;

namespace NadinSoft.Application.Features.Product.Commands;

public record DeleteProductByIdCommand(Guid Id, Guid UserId): IRequest<OperationResult<bool>>, IValidatableModel<DeleteProductByIdCommand>
{
    public IValidator<DeleteProductByIdCommand> Validate(ValidationModelBase<DeleteProductByIdCommand> validator)
    {
        validator.RuleFor(c => c.Id).NotEmpty();
        validator.RuleFor(c => c.UserId).NotEmpty();
        return validator;
    }
}