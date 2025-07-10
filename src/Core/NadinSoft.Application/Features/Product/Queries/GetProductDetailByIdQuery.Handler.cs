using AutoMapper;
using Mediator;
using NadinSoft.Application.Common;
using NadinSoft.Application.Repositories.Common;
using NadinSoft.Domain.Entities.Product;

namespace NadinSoft.Application.Features.Product.Queries;

public class GetProductDetailByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetProductDetailByIdQuery, OperationResult<GetProductDetailByIdResult>>
{
    public async ValueTask<OperationResult<GetProductDetailByIdResult>> Handle(GetProductDetailByIdQuery request,
        CancellationToken cancellationToken)
    {
        var product = await unitOfWork.ProductRepository.GetProductDetailByIdAsync(request.Id, cancellationToken);
        if (product is null)
            return OperationResult<GetProductDetailByIdResult>.NotFoundResult(nameof(GetProductDetailByIdQuery.Id),
                "Product not found");
        var result = mapper.Map<ProductEntity, GetProductDetailByIdResult>(product);
        return OperationResult<GetProductDetailByIdResult>.SuccessResult(result);
    }
}