using Asp.Versioning;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using NadinSoft.Application.Contracts.User.Models;
using NadinSoft.Application.Features.User.Commands.Register;
using NadinSoft.Application.Features.User.Queries.PasswordLogin;
using NadinSoft.WebFramework.Common;
using NadinSoft.WebFramework.Models;

namespace NadinSoft.Api.Controllers.User.V1;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/users")]
public class UserController(ISender sender) : BaseController
{
    [HttpPost("Register")]
    [ProducesResponseType(typeof(ApiResult), StatusCodes.Status200OK)]
    public virtual async Task<IActionResult> Register(RegisterUserCommand command,
        CancellationToken cancellationToken = default) =>
        base.OperationResult(await sender.Send(command, cancellationToken));

    [HttpPost("TokenRequest")]
    [ProducesResponseType(typeof(ApiResult<JwtAccessTokenModel>), StatusCodes.Status200OK)]
    public virtual async Task<IActionResult> TokenRequest(UserPasswordLoginQuery query,
        CancellationToken cancellationToken = default) => base.OperationResult(await sender.Send(query, cancellationToken));
}