using Asp.Versioning;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NadinSoft.Api.Models.Product;
using NadinSoft.Application.Features.Product.Commands;
using NadinSoft.Application.Features.Product.Queries;
using NadinSoft.WebFramework.Common;
using NadinSoft.WebFramework.Models;

namespace NadinSoft.Api.Controllers.Product.V1;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/products")]
public class ProductController(ISender sender) : BaseController
{
    [HttpPost("Create")]
    [ProducesResponseType(typeof(ApiResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> Create(CreateProductApiModel model, CancellationToken cancellationToken = default)
        => base.OperationResult(await sender.Send(
            new CreateProductCommand(model.Name, model.ManufacturePhone, model.ManufactureEmail, model.ProduceDate,
                base.UserId!.Value), cancellationToken));

    [HttpGet("UserProducts")]
    [ProducesResponseType(typeof(ApiResult<List<GetUserProductsQueryResult>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserProducts(CancellationToken cancellationToken = default)
        => base.OperationResult(await sender.Send(new GetUserProductsQuery(base.UserId!.Value), cancellationToken));


    [HttpPut("EditProduct")]
    [ProducesResponseType(typeof(ApiResult), StatusCodes.Status200OK)]
    public virtual async Task<IActionResult> EditProduct(EditProductCommand command,
        CancellationToken cancellationToken = default)
        => base.OperationResult(await sender.Send(command with { UserId = base.UserId!.Value}, cancellationToken));


    [HttpGet("Detail")]
    [ProducesResponseType(typeof(ApiResult<GetProductDetailByIdQueryResult>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDetailById(Guid id, CancellationToken cancellationToken = default)
        => base.OperationResult(await sender.Send(new GetProductDetailByIdQuery(id), cancellationToken));

    [HttpDelete("Delete")]
    [ProducesResponseType(typeof(ApiResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken = default)
        => base.OperationResult(await sender.Send(new DeleteProductByIdCommand(id, base.UserId!.Value), cancellationToken));

    [AllowAnonymous]
    [HttpGet("GetAllProduct")]
    [ProducesResponseType(typeof(ApiResult<List<GetAllProductsQueryResult>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllProducts(CancellationToken cancellationToken = default)
        => base.OperationResult(await sender.Send(new GetAllProductsQuery(), cancellationToken));
}