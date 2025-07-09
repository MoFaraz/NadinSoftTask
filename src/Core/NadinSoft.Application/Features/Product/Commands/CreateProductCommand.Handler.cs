using Mediator;
using NadinSoft.Application.Common;
using NadinSoft.Application.Repositories.Common;
using NadinSoft.Domain.Entities.Product;

namespace NadinSoft.Application.Features.Product.Commands;

public class CreateProductCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<CreateProductCommand, OperationResult<bool>>
{
    public async ValueTask<OperationResult<bool>> Handle(CreateProductCommand request,
        CancellationToken cancellationToken)
    {
        if (await unitOfWork.ProductRepository.IsProductExistsAsync(request.Name, cancellationToken))
            return OperationResult<bool>.FailureResult(nameof(request.Name),
                "This Product Already Exists");

        var product = ProductEntity.Create(request.Id, request.Name, request.ManufacturePhone,
            request.ManufactureEmail, request.ProduceDate, request.UserId);

        await unitOfWork.ProductRepository.CreateAsync(product, cancellationToken);

        await unitOfWork.CommitAsync(cancellationToken);
        return OperationResult<bool>.SuccessResult(true);
    }
}