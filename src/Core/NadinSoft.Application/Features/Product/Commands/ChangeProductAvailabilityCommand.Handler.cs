using Mediator;
using NadinSoft.Application.Common;
using NadinSoft.Application.Repositories.Common;

namespace NadinSoft.Application.Features.Product.Commands;

public class ChangeProductAvailabilityCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<ChangeProductAvailabilityCommand, OperationResult<bool>>
{
    public async ValueTask<OperationResult<bool>> Handle(ChangeProductAvailabilityCommand request,
        CancellationToken cancellationToken)
    {
        var product = await unitOfWork.ProductRepository.GetProductByIdForUpdateAsync(request.Id, cancellationToken);

        if (product is null)
            return OperationResult<bool>.NotFoundResult(nameof(ChangeProductAvailabilityCommand.Id),
                "Product not found");

        if (!product.UserId.Equals(request.UserId))
            return OperationResult<bool>.FailureResult(nameof(ChangeProductAvailabilityCommand.UserId),
                "User Id does not match");

        product.ChangeAvailability(request.IsAvailable);
        await unitOfWork.CommitAsync(cancellationToken);
        return OperationResult<bool>.SuccessResult(true);
    }
}