using Mediator;
using NadinSoft.Application.Common;

namespace NadinSoft.Application.Features.Product.Queries;

public record GetAllProductsQuery():IRequest<OperationResult<List<GetAllProductsQueryResult>>>;