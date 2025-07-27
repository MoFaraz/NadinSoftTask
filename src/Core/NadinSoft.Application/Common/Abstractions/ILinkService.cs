namespace NadinSoft.Application.Common.Abstractions;

public interface ILinkService
{
    Link GenerateLink(string endpointName, object? routeValues, string rel, string method);
}