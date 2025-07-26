using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NadinSoft.WebFramework.Models;

namespace NadinSoft.WebFramework.Filters;

public class ForbiddenResultAttribute: ResultFilterAttribute
{
    public override void OnResultExecuting(ResultExecutingContext context)
    {
        if (context.Result is ForbidResult forbiddenResult)
        {
            var apiResult = new ApiResult(false, ApiResultStatusCode.Forbidden.ToString("G"),
                ApiResultStatusCode.Forbidden);

            context.Result = new JsonResult(apiResult)
            {
                StatusCode = StatusCodes.Status403Forbidden
            };
        }
    }
}