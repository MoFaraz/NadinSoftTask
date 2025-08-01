using NadinSoft.WebFramework.Extensions;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;

namespace NadinSoft.WebFramework.Swagger;

public class ApiVersionDocumentProcessor : IDocumentProcessor
{
    public void Process(DocumentProcessorContext context)
    {
        var version = context.Document.Info.Version;

        var pathsToRemove =
            context.Document.Paths.Where(pathItem => !RegexHelpers.MatchesApiVersion(version, pathItem.Key))
                .Select(c => c.Key).ToList();

        foreach (var pathToRemove in pathsToRemove)
        {
            context.Document.Paths.Remove(pathToRemove);
        }
    }
}