using FluentValidation;
using Mediator;
using NadinSoft.Application.Common;
using NadinSoft.Application.Common.Validation;
using NadinSoft.Domain.Entities.Product;

namespace NadinSoft.Application.Features.Product.Queries;

public record GetProductByIdQuery(Guid Id): IRequest<OperationResult<GetProductByIdQueryResult>>, IValidatableModel<GetProductByIdQuery>
{
    public IValidator<GetProductByIdQuery> Validate(ValidationModelBase<GetProductByIdQuery> validator)
    {
       validator.RuleFor(c => c.Id).NotEmpty().WithMessage("Id is required."); 
       
       return validator;
    }
}