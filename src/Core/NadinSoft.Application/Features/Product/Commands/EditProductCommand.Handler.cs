using Mediator;
using NadinSoft.Application.Common;
using NadinSoft.Application.Repositories.Common;

namespace NadinSoft.Application.Features.Product.Commands;

public class EditProductCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<EditProductCommand, OperationResult<bool>>
{
    public async ValueTask<OperationResult<bool>> Handle(EditProductCommand request,
        CancellationToken cancellationToken)
    {
        var product = await unitOfWork.ProductRepository.GetProductByIdForUpdateAsync(request.Id, cancellationToken);

        if (product is null)
            return OperationResult<bool>.NotFoundResult(nameof(EditProductCommand.Id), "Product not found");
        
        if (!product.UserId.Equals(request.UserId))
            return OperationResult<bool>.FailureResult(nameof(EditProductCommand.UserId), "User Id does not match");

        product.Edit(request.Name, request.ManufacturePhone, request.ManufactureEmail, request.ProduceDate);

        await unitOfWork.CommitAsync(cancellationToken);

        return OperationResult<bool>.SuccessResult(true);
    }
}