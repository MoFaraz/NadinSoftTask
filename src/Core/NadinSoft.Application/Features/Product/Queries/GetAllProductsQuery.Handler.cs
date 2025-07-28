using Mediator;
using NadinSoft.Application.Common;
using NadinSoft.Application.Common.Abstractions;
using NadinSoft.Application.Repositories.Common;

namespace NadinSoft.Application.Features.Product.Queries;

public class GetAllProductsQueryHandler(IUnitOfWork unitOfWork, ILinkService linkService)
    : IRequestHandler<GetAllProductsQuery, OperationResult<PageResult<GetAllProductsQueryResult>>>
{
    public async ValueTask<OperationResult<PageResult<GetAllProductsQueryResult>>> Handle(GetAllProductsQuery request,
        CancellationToken cancellationToken)
    {
        var products =
            await unitOfWork.ProductRepository.GetAllAsync(request.Username, request.Page, request.PageSize,
                cancellationToken);

        var result = products.Map(c => new GetAllProductsQueryResult(c.Id, c.Name, c.ManufacturePhone,
            c.ManufactureEmail, c.IsAvailable,
            c.User.UserName!, c.ProduceDate));
        
        AddLinksForPagedProducts(request, result);

        return OperationResult<PageResult<GetAllProductsQueryResult>>.SuccessResult(result);
    }
    
    private void AddLinksForPagedProducts(GetAllProductsQuery query, PageResult<GetAllProductsQueryResult> pageResult)
    {
        if (pageResult.HasNextPage)
        {
            pageResult.Links.Add(linkService.GenerateLink("GetAllProducts",
                new { userName = query.Username, pageSize = query.PageSize, page = query.Page + 1 },
                "next-page",
                "GET"
            ));
        }

        if (pageResult.HasPreviousPage)
        {
            pageResult.Links.Add(linkService.GenerateLink("GetAllProducts",
                new { userName = query.Username, pageSize = query.PageSize, page = query.Page - 1 },
                "previous-page",
                "GET"
            ));
        }
    }
}
