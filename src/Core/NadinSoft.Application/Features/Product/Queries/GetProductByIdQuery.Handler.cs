using Mediator;
using NadinSoft.Application.Common;
using NadinSoft.Application.Repositories.Common;
using NadinSoft.Domain.Entities.Product;

namespace NadinSoft.Application.Features.Product.Queries;

public class GetProductByIdQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetProductByIdQuery, OperationResult<GetProductByIdQueryResult>>
{
    public async ValueTask<OperationResult<GetProductByIdQueryResult>> Handle(GetProductByIdQuery request,
        CancellationToken cancellationToken)
    {
        var product = await unitOfWork.ProductRepository.GetProductByIdForUpdateAsync(request.Id, cancellationToken);
        if (product is null)
            return OperationResult<GetProductByIdQueryResult>.NotFoundResult(nameof(GetProductByIdQuery.Id),
                "Product not found");

        return OperationResult<GetProductByIdQueryResult>.SuccessResult(new GetProductByIdQueryResult(product.Name,
            product.ManufacturePhone,
            product.ManufactureEmail, product.IsAvailable, product.User.UserName!, product.ProduceDate));
    }
}