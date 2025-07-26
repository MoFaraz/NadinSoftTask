using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NadinSoft.Application.Common;
using NadinSoft.WebFramework.Extensions;

namespace NadinSoft.WebFramework.Common;

public class BaseController : Controller
{
    protected string? UserName => base.User.Identity?.Name;

    protected Guid? UserId => Guid.Parse(User.Identity?.GetUserId()!);

    protected string UserEmail => User.Identity?.FindFirstValue(ClaimTypes.Email)!;

    protected string UserKey => User.Identity?.FindFirstValue(ClaimTypes.UserData)!;

    protected IActionResult OperationResult<TModel>(OperationResult<TModel> result)
    {
        if (result.IsSuccess)
            return result.Result is bool ? Ok() : Ok(result.Result);

        if (result.IsNotFound)
        {
            AddErrors(result);

            var errors = new ValidationProblemDetails(ModelState);
            return NotFound(errors);
        }

        if (result.IsForbidden)
        {
            AddErrors(result);
            var errors = new ValidationProblemDetails(ModelState);
            return StatusCode(StatusCodes.Status403Forbidden, errors);
        }
        
        AddErrors(result);
        var badRequestErrors = new ValidationProblemDetails(ModelState);
        
        return BadRequest(badRequestErrors);
    }

    private void AddErrors<TModel>(OperationResult<TModel> result)
    {
        foreach (var errorMessage in result.ErrorMessages)
        {
            ModelState.AddModelError(errorMessage.Key, errorMessage.Value);
        }
    }
}