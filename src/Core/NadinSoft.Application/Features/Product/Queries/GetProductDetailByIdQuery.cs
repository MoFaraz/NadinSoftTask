using FluentValidation;
using Mediator;
using NadinSoft.Application.Common;
using NadinSoft.Application.Common.Validation;

namespace NadinSoft.Application.Features.Product.Queries;

public record GetProductDetailByIdQuery(Guid Id): IRequest<OperationResult<GetProductDetailByIdQueryResult>>, IValidatableModel<GetProductDetailByIdQuery>
{
    public IValidator<GetProductDetailByIdQuery> Validate(ValidationModelBase<GetProductDetailByIdQuery> validator)
    {
        validator.RuleFor(c => c.Id).NotEmpty();
        return validator;
    }
}