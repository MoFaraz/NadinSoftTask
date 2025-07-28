using FluentValidation;
using Mediator;
using NadinSoft.Application.Common;
using NadinSoft.Application.Common.Validation;

namespace NadinSoft.Application.Features.Product.Queries;

public record GetUserProductsQuery(Guid UserId, int Page = 1, int PageSize = 10): IRequest<OperationResult<PageResult<GetUserProductsQueryResult>>>, IValidatableModel<GetUserProductsQuery>
{
    public IValidator<GetUserProductsQuery> Validate(ValidationModelBase<GetUserProductsQuery> validator)
    {
        validator.RuleFor(c => c.UserId).NotEmpty();
        validator.RuleFor(c => c.Page).GreaterThan(0).WithMessage("Page must be greater than 0");
        validator.RuleFor(c => c.PageSize).GreaterThan(0).WithMessage("PageSize must be greater than 0");
        
        return validator;
    }
}