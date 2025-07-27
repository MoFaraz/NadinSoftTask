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
        if (await unitOfWork.ProductRepository.IsUniqueManufactureEmailAndProductDate(request.ManufactureEmail,
                request.ProduceDate, cancellationToken))
            return OperationResult<bool>.FailureResult("ProductDate and ManufactureEmail",
                "ProductDate and ManufactureEmail Can not be Duplicated");

        var newProduct = ProductEntity.Create(request.Name, request.ManufacturePhone,
            request.ManufactureEmail, request.ProduceDate, request.UserId);

        await unitOfWork.ProductRepository.CreateAsync(newProduct, cancellationToken);

        await unitOfWork.CommitAsync(cancellationToken);
        return OperationResult<bool>.SuccessResult(true);
    }
}