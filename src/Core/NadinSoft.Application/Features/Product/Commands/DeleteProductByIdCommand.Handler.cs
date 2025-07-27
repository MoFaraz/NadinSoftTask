using Mediator;
using NadinSoft.Application.Common;
using NadinSoft.Application.Repositories.Common;

namespace NadinSoft.Application.Features.Product.Commands;

public class DeleteProductByIdCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteProductByIdCommand, OperationResult<bool>>
{
    public async ValueTask<OperationResult<bool>> Handle(DeleteProductByIdCommand request,
        CancellationToken cancellationToken)
    {
        var product = await unitOfWork.ProductRepository.GetProductByIdForUpdateAsync(request.Id, cancellationToken);

        if (product is null)
            return OperationResult<bool>.NotFoundResult(nameof(DeleteProductByIdCommand.Id), "Product not found");

        if (!product.UserId.Equals(request.UserId))
            return OperationResult<bool>.ForbiddenResult(nameof(DeleteProductByIdCommand.UserId),
                "You Can Only Delete Products That You Have Created.");

        await unitOfWork.ProductRepository.DeleteByIdAsync(request.Id, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        return OperationResult<bool>.SuccessResult(true);
    }
}