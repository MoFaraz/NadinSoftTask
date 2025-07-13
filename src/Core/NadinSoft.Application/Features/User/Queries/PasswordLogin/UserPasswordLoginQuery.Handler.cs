using Mediator;
using NadinSoft.Application.Common;
using NadinSoft.Application.Contracts.User;
using NadinSoft.Application.Contracts.User.Models;
using NadinSoft.Application.Extensions;

namespace NadinSoft.Application.Features.User.Queries.PasswordLogin;

public class UserPasswordLoginQueryHandler(IUserManager userManager, IJwtService jwtService)
    : IRequestHandler<UserPasswordLoginQuery, OperationResult<JwtAccessTokenModel>>
{
    public async ValueTask<OperationResult<JwtAccessTokenModel>> Handle(UserPasswordLoginQuery request,
        CancellationToken cancellationToken)
    {
        var user = request.UserNameOrEmail.IsEmail()
            ? await userManager.GetUserByEmailAsync(request.UserNameOrEmail, cancellationToken)
            : await userManager.GetUserByUserNameAsync(request.UserNameOrEmail, cancellationToken);

        if (user is null)
            return OperationResult<JwtAccessTokenModel>.NotFoundResult(nameof(UserPasswordLoginQuery.UserNameOrEmail),
                "User Not Found");

        var passwordValidation = await userManager.ValidatePasswordAsync(user, request.Password, cancellationToken);
        if (passwordValidation.Succeeded)
        {
            var accessToken = await jwtService.GenerateTokenAsync(user, cancellationToken);
            return OperationResult<JwtAccessTokenModel>.SuccessResult(accessToken);
        }

        return OperationResult<JwtAccessTokenModel>.FailureResult(nameof(UserPasswordLoginQuery.Password),
            "Invalid Password");
    }
}