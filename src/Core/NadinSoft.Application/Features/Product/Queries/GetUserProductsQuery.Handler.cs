using Mediator;
using NadinSoft.Application.Common;
using NadinSoft.Application.Common.Abstractions;
using NadinSoft.Application.Repositories.Common;

namespace NadinSoft.Application.Features.Product.Queries;

public class GetUserProductsQueryHandler(IUnitOfWork unitOfWork, ILinkService linkService)
    : IRequestHandler<GetUserProductsQuery, OperationResult<PageResult<GetUserProductsQueryResult>>>
{
    public async ValueTask<OperationResult<PageResult<GetUserProductsQueryResult>>> Handle(GetUserProductsQuery request,
        CancellationToken cancellationToken)
    {
        var products = await unitOfWork.ProductRepository.GetUserProductsAsync(request.UserId, request.Page,
            request.PageSize, cancellationToken);

        var result = products.Map(c =>
            new GetUserProductsQueryResult(c.Id, c.Name, c.ManufacturePhone, c.ManufactureEmail, c.IsAvailable,
                c.ProduceDate));
        
        AddLinksForPagedProducts(request, result);

        return OperationResult<PageResult<GetUserProductsQueryResult>>.SuccessResult(result);
    }

    private void AddLinksForPagedProducts(GetUserProductsQuery query,
        PageResult<GetUserProductsQueryResult> pageResult)
    {
        if (pageResult.HasNextPage)
        {
            pageResult.Links.Add(linkService.GenerateLink("GetUserProducts",
                new { pageSize = query.PageSize, page = query.Page + 1 },
                "next-page",
                "GET"
            ));
        }

        if (pageResult.HasPreviousPage)
        {
            pageResult.Links.Add(linkService.GenerateLink("GetUserProducts",
                new { pageSize = query.PageSize, page = query.Page - 1 },
                "previous-page",
                "GET"
            ));
        }
    }
}