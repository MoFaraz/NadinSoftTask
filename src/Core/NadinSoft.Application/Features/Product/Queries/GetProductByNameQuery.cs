using FluentValidation;
using Mediator;
using NadinSoft.Application.Common;
using NadinSoft.Application.Common.Validation;

namespace NadinSoft.Application.Features.Product.Queries;

public record GetProductByNameQuery(string SearchTerm, int Page = 1, int PageSize = 10): IValidatableModel<GetProductByNameQuery>, IRequest<OperationResult<PageResult<GetProductByNameQueryResult>>>
{
    public IValidator<GetProductByNameQuery> Validate(ValidationModelBase<GetProductByNameQuery> validator)
    {
        validator.RuleFor(c => c.SearchTerm).NotEmpty().MinimumLength(3).WithMessage("Name is required.");
        validator.RuleFor(c => c.Page).GreaterThan(0).WithMessage("Page Must Be Greater Than Zero.");
        validator.RuleFor(c => c.PageSize).GreaterThan(0).WithMessage("Page Size Must Be Greater Than Zero.");
        
        return validator;
    }
}