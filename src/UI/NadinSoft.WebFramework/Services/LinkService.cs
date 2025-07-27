using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using NadinSoft.Application.Common.Abstractions;

namespace NadinSoft.WebFramework.Services;

public class LinkService(LinkGenerator linkGenerator, IHttpContextAccessor httpContextAccessor)
    : ILinkService
{
    public Link GenerateLink(string endpointName, object? routeValues, string rel, string method)
    {
        return new Link(linkGenerator.GetUriByName(httpContextAccessor.HttpContext, endpointName, routeValues), rel, method);
    }
}