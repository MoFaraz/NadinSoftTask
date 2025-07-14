using Mediator;
using NadinSoft.Application.Common;
using NadinSoft.Application.Contracts.User;
using NadinSoft.Application.Extensions;
using NadinSoft.Domain.Entities.User;

namespace NadinSoft.Application.Features.User.Commands.Register;

public class RegisterUserCommandHandler(IUserManager userManager)
    : IRequestHandler<RegisterUserCommand, OperationResult<bool>>
{
    public async ValueTask<OperationResult<bool>> Handle(RegisterUserCommand request, CancellationToken cancellationToken = default)
    {
        var user = new UserEntity(request.FirstName, request.LastName, request.Username, request.Email);

        var userCreateResult = await userManager.PasswordCreateAsync(user, request.Password, cancellationToken);
        if (userCreateResult.Succeeded)
            return OperationResult<bool>.SuccessResult(true);
        
        return OperationResult<bool>.FailureResult(userCreateResult.Errors.ConvertToKeyValuePair());
    }
}