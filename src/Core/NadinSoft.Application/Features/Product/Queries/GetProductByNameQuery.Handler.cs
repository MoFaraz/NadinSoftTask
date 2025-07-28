using Mediator;
using NadinSoft.Application.Common;
using NadinSoft.Application.Common.Abstractions;
using NadinSoft.Application.Repositories.Common;

namespace NadinSoft.Application.Features.Product.Queries;

public class GetProductByNameQueryHandler(IUnitOfWork unitOfWork, ILinkService linkService)
    : IRequestHandler<GetProductByNameQuery, OperationResult<PageResult<GetProductByNameQueryResult>>>
{
    public async ValueTask<OperationResult<PageResult<GetProductByNameQueryResult>>> Handle(
        GetProductByNameQuery request,
        CancellationToken cancellationToken)
    {
        var product = await unitOfWork.ProductRepository.GetByNameAsync(request.SearchTerm, request.Page,
            request.PageSize, cancellationToken);

        var result = product.Map(c => new GetProductByNameQueryResult(c.Id, c.Name, c.ManufacturePhone,
            c.ManufactureEmail,
            c.IsAvailable, c.ProduceDate,
            c.UserId));

        AddLinksForPagedProducts(request, result);
        return OperationResult<PageResult<GetProductByNameQueryResult>>.SuccessResult(result);
    }

    private void AddLinksForPagedProducts(GetProductByNameQuery query,
        PageResult<GetProductByNameQueryResult> pageResult)
    {
        if (pageResult.HasNextPage)
        {
            pageResult.Links.Add(linkService.GenerateLink("SearchProducts",
                new { SearchTerm = query.SearchTerm, pageSize = query.PageSize, page = query.Page + 1 },
                "next-page",
                "GET"
            ));
        }

        if (pageResult.HasPreviousPage)
        {
            pageResult.Links.Add(linkService.GenerateLink("SearchProducts",
                new { SearchTerm = query.SearchTerm, pageSize = query.PageSize, page = query.Page - 1 },
                "previous-page",
                "GET"
            ));
        }
    }
}