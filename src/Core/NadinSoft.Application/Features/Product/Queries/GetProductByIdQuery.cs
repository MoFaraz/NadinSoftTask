using FluentValidation;
using Mediator;
using NadinSoft.Application.Common;
using NadinSoft.Application.Common.Validation;

namespace NadinSoft.Application.Features.Product.Queries;

public record GetProductByIdQuery(Guid Id): IRequest<OperationResult<GetProductDetailByIdQueryResult>>, IValidatableModel<GetProductByIdQuery>
{
    public IValidator<GetProductByIdQuery> Validate(ValidationModelBase<GetProductByIdQuery> validator)
    {
        validator.RuleFor(c => c.Id).NotEmpty();
        return validator;
    }
}