using FluentValidation;
using Mediator;
using NadinSoft.Application.Common;
using NadinSoft.Application.Common.Validation;

namespace NadinSoft.Application.Features.Product.Queries;

public record GetAllProductsQuery() : IRequest<OperationResult<List<GetAllProductsQueryResult>>>,
    IValidatableModel<GetAllProductsQuery>
{
    public IValidator<GetAllProductsQuery> Validate(ValidationModelBase<GetAllProductsQuery> validator)
    {
        return validator;
    }
}