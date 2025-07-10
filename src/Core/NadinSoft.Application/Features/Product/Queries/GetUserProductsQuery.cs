using FluentValidation;
using Mediator;
using NadinSoft.Application.Common;
using NadinSoft.Application.Common.Validation;

namespace NadinSoft.Application.Features.Product.Queries;

public record GetUserProductsQuery(Guid UserId): IRequest<OperationResult<List<GetUserProductsQueryResult>>>, IValidatableModel<GetUserProductsQuery>
{
    public IValidator<GetUserProductsQuery> Validate(ValidationModelBase<GetUserProductsQuery> validator)
    {
        validator.RuleFor(c => c.UserId).NotEmpty();
        
        return validator;
    }
}