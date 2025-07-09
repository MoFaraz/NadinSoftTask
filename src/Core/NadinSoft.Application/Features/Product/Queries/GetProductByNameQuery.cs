using FluentValidation;
using Mediator;
using NadinSoft.Application.Common;
using NadinSoft.Application.Common.Validation;

namespace NadinSoft.Application.Features.Product.Queries;

public record GetProductByNameQuery(string SearchTerm): IValidatableModel<GetProductByNameQuery>, IRequest<OperationResult<List<GetProductByNameQueryResult>>>
{
    public IValidator<GetProductByNameQuery> Validate(ValidationModelBase<GetProductByNameQuery> validator)
    {
        validator.RuleFor(c => c.SearchTerm).NotEmpty().MinimumLength(3).WithMessage("Name is required.");
        return validator;
    }
}