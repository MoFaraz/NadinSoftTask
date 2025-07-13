using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NadinSoft.WebFramework.Models;

namespace NadinSoft.WebFramework.Filters;

public class NotFoundAttribute : ResultFilterAttribute
{
    public override void OnResultExecuting(ResultExecutingContext context)
    {
        if (context.Result is NotFoundObjectResult notFoundObjectResult)
        {
            var apiResult = new ApiResult<object>(false, ApiResultStatusCode.NotFound.ToString("G"),
                ApiResultStatusCode.NotFound,
                notFoundObjectResult.Value!);

            context.Result = new JsonResult(apiResult) { StatusCode = StatusCodes.Status404NotFound };
            return;
        }

        if (context.Result is NotFoundResult notFoundResult)
        {
            var apiResult = new ApiResult(false, ApiResultStatusCode.NotFound.ToString("G"),
                ApiResultStatusCode.NotFound);
            
            context.Result = new JsonResult(apiResult) { StatusCode = StatusCodes.Status404NotFound };
        }
    }
}