using AutoMapper;
using Mediator;
using NadinSoft.Application.Common;
using NadinSoft.Application.Common.Abstractions;
using NadinSoft.Application.Repositories.Common;
using NadinSoft.Domain.Entities.Product;

namespace NadinSoft.Application.Features.Product.Queries;

public class GetProductDetailByIdHandler(IUnitOfWork unitOfWork, IMapper mapper, ILinkService linkService)
    : IRequestHandler<GetProductByIdQuery, OperationResult<GetProductDetailByIdQueryResult>>
{
    public async ValueTask<OperationResult<GetProductDetailByIdQueryResult>> Handle(GetProductByIdQuery request,
        CancellationToken cancellationToken)
    {
        var product = await unitOfWork.ProductRepository.GetProductDetailByIdAsync(request.Id, cancellationToken);
        if (product is null)
            return OperationResult<GetProductDetailByIdQueryResult>.NotFoundResult(nameof(GetProductByIdQuery.Id),
                "Product not found");
        var result = mapper.Map<ProductEntity, GetProductDetailByIdQueryResult>(product);
        AddLinksForProduct(result);
        return OperationResult<GetProductDetailByIdQueryResult>.SuccessResult(result);
    }

    private void AddLinksForProduct(GetProductDetailByIdQueryResult result)
    {
        result.Links.Add(linkService.GenerateLink("GetProduct", new { Id = result.Id }, "self", "GET"));
        result.Links.Add(linkService.GenerateLink("DeleteProduct", new { Id = result.Id }, "delete-product", "DELETE"));
        result.Links.Add(linkService.GenerateLink("UpdateProduct", new { Id = result.Id }, "update-product", "PUT"));
        result.Links.Add(linkService.GenerateLink("ChangeAvailability", new { Id = result.Id }, "change-availability",
            "POST"));
    }
}