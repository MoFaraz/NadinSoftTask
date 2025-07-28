using FluentValidation;
using Mediator;
using NadinSoft.Application.Common;
using NadinSoft.Application.Common.Validation;

namespace NadinSoft.Application.Features.Product.Queries;

public record GetAllProductsQuery(string? Username, int Page = 1, int PageSize = 10)
    : IRequest<OperationResult<PageResult<GetAllProductsQueryResult>>>,
        IValidatableModel<GetAllProductsQuery>
{
    public IValidator<GetAllProductsQuery> Validate(ValidationModelBase<GetAllProductsQuery> validator)
    {
        validator.RuleFor(c => c.Page).GreaterThanOrEqualTo(1).WithMessage("Page Number must be greater than 1.");
        validator.RuleFor(c => c.PageSize).GreaterThanOrEqualTo(1).WithMessage("Page Size must be greater than 1.");
        return validator;
    }
}