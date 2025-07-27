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
[Authorize]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/products")]
public class ProductController(ISender sender) : BaseController
{
    [HttpPost("")]
    [EndpointName("CreateProduct")]
    [ProducesResponseType(typeof(ApiResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> Create(CreateProductApiModel model, CancellationToken cancellationToken = default)
        => OperationResult(await sender.Send(
            new CreateProductCommand(model.Name, model.ManufacturePhone, model.ManufactureEmail, model.ProduceDate,
                UserId!.Value), cancellationToken));

    [HttpGet("user-product")]
    [EndpointName("GetUserProducts")]
    [ProducesResponseType(typeof(ApiResult<List<GetUserProductsQueryResult>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserProducts(CancellationToken cancellationToken = default)
        => OperationResult(await sender.Send(new GetUserProductsQuery(UserId!.Value), cancellationToken));


    [HttpPut("{id}")]
    [EndpointName("UpdateProduct")]
    [ProducesResponseType(typeof(ApiResult), StatusCodes.Status200OK)]
    public virtual async Task<IActionResult> EditProduct(Guid id, EditProductApiModel model,
        CancellationToken cancellationToken = default)
        => OperationResult(await sender.Send(
            new EditProductCommand(id, model.Name, model.ManufacturePhone, model.ManufactureEmail,
                model.ProduceDate, UserId!.Value), cancellationToken));


    [HttpGet("{id}")]
    [EndpointName("GetProduct")]
    [ProducesResponseType(typeof(ApiResult<GetProductDetailByIdQueryResult>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDetailById(Guid id, CancellationToken cancellationToken = default)
        => OperationResult(await sender.Send(new GetProductByIdQuery(id), cancellationToken));

    [HttpDelete("{id}")]
    [EndpointName("DeleteProduct")]
    [ProducesResponseType(typeof(ApiResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken = default)
        => OperationResult(await sender.Send(new DeleteProductByIdCommand(id, UserId!.Value),
            cancellationToken));

    [HttpPost("change-availability/{id}")]
    [EndpointName("ChangeAvailability")]
    [ProducesResponseType(typeof(ApiResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> ChangeAvailability(Guid id,ChangeAvailabilityApiModel model,
        CancellationToken cancellationToken = default)
        => OperationResult(
            await sender.Send(new ChangeProductAvailabilityCommand(id, model.Availability, UserId!.Value),
                cancellationToken));

    [AllowAnonymous]
    [HttpGet("")]
    [EndpointName("GetAllProducts")]
    [ProducesResponseType(typeof(ApiResult<List<GetAllProductsQueryResult>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllProducts([FromQuery] GetAllProductModel model,
        CancellationToken cancellationToken = default)
        => OperationResult(await sender.Send(new GetAllProductsQuery(model.Username, model.Page, model.PageSize),
            cancellationToken));
}