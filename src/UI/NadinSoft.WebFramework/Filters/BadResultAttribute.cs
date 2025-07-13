using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NadinSoft.WebFramework.Models;

namespace NadinSoft.WebFramework.Filters;

public class BadResultAttribute : ResultFilterAttribute
{
    public override void OnResultExecuting(ResultExecutingContext context)
    {
        if (context.Result is not BadRequestObjectResult badRequestObjectResult)
            return;

        var modelState = context.ModelState;

        if (!modelState.IsValid)
        {
            var errors = new ValidationProblemDetails(modelState);

            var apiResult = new ApiResult<IDictionary<string, string[]>>(false,
                ApiResultStatusCode.BadRequest.ToString("G"), ApiResultStatusCode.BadRequest,
                errors.Errors);

            context.Result = new BadRequestObjectResult(apiResult) { StatusCode = StatusCodes.Status400BadRequest };
            return;
        }

        var badRequestResult = new ApiResult<object>(false,
            ApiResultStatusCode.BadRequest.ToString("G"), ApiResultStatusCode.BadRequest,
            badRequestObjectResult.Value!);

        context.Result = new BadRequestObjectResult(badRequestResult) { StatusCode = StatusCodes.Status400BadRequest };
    }
}